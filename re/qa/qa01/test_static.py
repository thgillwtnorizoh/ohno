"""Pinned-source/CIL-byte QA only. No C# or application execution here."""
import hashlib
import json
import os
from pathlib import Path
import re
import unittest
import zipfile

HERE = Path(__file__).resolve().parent
ROOT = HERE.parents[2]
APP = ROOT / 'reconstructed/CircleGame'
PINS = json.loads((HERE / 'provenance.json').read_text())


def source(path):
    return (APP / path).read_text()


def compact(text):
    # Strip comments while retaining quoted literals. This is a deliberately
    # bounded source inspector, not a C# parser, compiler or gameplay model.
    out = []
    i = 0
    quote = None
    while i < len(text):
        ch = text[i]
        if quote:
            out.append(ch)
            if ch == '\\' and i + 1 < len(text):
                i += 1
                out.append(text[i])
            elif ch == quote:
                quote = None
        elif ch in ('"', "'"):
            quote = ch
            out.append(ch)
        elif text[i:i + 2] == '//':
            end = text.find('\n', i)
            i = len(text) if end < 0 else end
            continue
        elif text[i:i + 2] == '/*':
            end = text.find('*/', i + 2)
            if end < 0:
                raise ValueError('unterminated source comment')
            i = end + 2
            continue
        else:
            out.append(ch)
        i += 1
    return re.sub(r'\s+', '', ''.join(out))


SESSION = compact(source('Gameplay/CircleSession.cs'))
PARSER = compact(source('Gameplay/Beatmap.cs'))
VIEW = compact(source('Rendering/PlayfieldView.cs'))
WINDOW = compact(source('Program.cs'))
LAYOUT = compact(source('Rendering/PlayfieldLayout.cs'))


class SourceContracts(unittest.TestCase):
    def test_01_pinned_work2_sources(self):
        for path, digest in PINS['work2_sha256'].items():
            with self.subTest(path=path):
                self.assertEqual(hashlib.sha256((ROOT / path).read_bytes()).hexdigest(), digest)

    def test_02_window_anchors(self):
        for term in ['MapDifficulty(map.OverallDifficulty,80,50,20)',
                     'MapDifficulty(map.OverallDifficulty,140,100,60)',
                     'MapDifficulty(map.OverallDifficulty,200,150,100)']:
            self.assertIn(term, SESSION)

    def test_03_strict_classification_order(self):
        self.assertIn('error<Window300?Judgement.Hit300:error<Window100?Judgement.Hit100:'
                      'error<Window50?Judgement.Hit50:Judgement.Miss', SESSION)

    def test_04_absolute_integer_error(self):
        self.assertIn('interror=Math.Abs(time-circle.Definition.StartTime);', SESSION)

    def test_05_expiration_is_strict(self):
        self.assertIn('circle.Result==Judgement.Pending&&time>circle.Definition.StartTime+Window50', SESSION)

    def test_06_evaluated_exclusion(self):
        self.assertIn('circle.Result!=Judgement.Pending||time<start-Approach||time>start+Window50', SESSION)

    def test_07_selection_includes_both_time_equalities(self):
        self.assertIn('time<start-Approach||time>start+Window50)continue;', SESSION)

    def test_08_first_passing_circle_returns(self):
        self.assertIn('Judge(circle,time);return;', SESSION)
        self.assertNotIn('OrderBy', SESSION)

    def test_09_admission_strict_400_and_earlier_three(self):
        self.assertIn('Math.Abs(time-start)>=400', SESSION)
        self.assertIn('c.Result==Judgement.Pending&&c.Definition.StartTime+3<start', SESSION)

    def test_10_approach_and_truncation(self):
        self.assertIn('MapDifficulty(map.ApproachRate,1800,1200,450)', SESSION)
        self.assertIn('(int)(d>5?b+(c-b)*(d-5)/5:d<5?b-(b-a)*(5-d)/5:b)', SESSION)

    def test_11_separate_radius_preparation(self):
        self.assertIn('0.699999988079071', SESSION)
        self.assertIn('HitRadius=(DiameterAtScaleOne/2f)*1.0004099607467651f;', SESSION)

    def test_12_squared_geometry_preserves_finite_equality(self):
        self.assertIn('Vector2.DistanceSquared(pointer,circle.Definition.Position)>HitRadius*HitRadius', SESSION)
        self.assertNotIn('Math.Sqrt', SESSION)

    def test_13_QA01_D01_reject_unordered_distance(self):
        press = SESSION.split('publicvoidPress(', 1)[1].split('privatevoidJudge(', 1)[0]
        finite_guard = 'float.IsFinite(pointer.X)' in press and 'float.IsFinite(pointer.Y)' in press
        inverted_ordered = 'if(!(Vector2.DistanceSquared(pointer,circle.Definition.Position)<=HitRadius*HitRadius))continue;' in press
        self.assertTrue(finite_guard or inverted_ordered,
                        'QA01-D01: bare distanceSquared > radiusSquared rejection accepts NaN; '
                        'original M24c7:0054 requires ordered <=. No C# runtime observation claimed.')

    def test_14_forward_and_inverse_layout(self):
        self.assertIn('point*Scale+Offset+newVector2(StageX,StageY)', LAYOUT)
        self.assertIn('(point-newVector2(StageX,StageY)-Offset)*(1f/Scale)', LAYOUT)
        self.assertIn('((Height-384f*Scale)/4f)*3f-16f*Scale', LAYOUT)

    def test_15_surface_maps_stored_time_without_refresh(self):
        self.assertIn('Session.Press(layout.ToMap(newVector2((float)mouse.X,(float)mouse.Y)),CurrentTime)', VIEW)
        self.assertIn('CurrentTime=clock.Time;Session.Advance(CurrentTime);', VIEW)

    def test_16_input_repeat_and_held_key_policy(self):
        self.assertIn('if(e.IsRepeat||!heldKeys.Add(e.Key))return;', WINDOW)
        self.assertIn('caseKey.Z:caseKey.X:surface.Press();break;', WINDOW)

    def test_17_key_release_and_deactivation(self):
        self.assertIn('PreviewKeyUp+=(_,e)=>heldKeys.Remove(e.Key);', WINDOW)
        self.assertIn('heldKeys.Clear();surface.Pause();UpdateHud();', WINDOW)

    def test_18_paused_input_is_guarded_before_mouse_sample(self):
        guard = VIEW.index('if(!Running||Sessionisnull||ActualWidth<=0||ActualHeight<=0)return;')
        self.assertLess(guard, VIEW.index('Mouse.GetPosition(this)'))

    def test_19_no_mouse_click_judgement(self):
        self.assertIn('MouseDown+=(_,_)=>Focus();', VIEW)

    def test_20_fresh_per_session_circle_states(self):
        self.assertIn('map.Circles.Select(c=>newCircleState(c)).ToArray()', SESSION)
        self.assertIn('Session=new(Session.Map);started=true;CurrentTime=0;clock.Restart();', VIEW)

    def test_21_map_load_waits_and_pauses(self):
        self.assertIn('clock.Pause();Session=new(map);CurrentTime=0;started=false;', VIEW)

    def test_22_parse_before_replacing_session(self):
        self.assertIn('Beatmapmap=Beatmap.Load(path);surface.Load(map);', WINDOW)

    def test_23_result_and_finished_guards(self):
        self.assertEqual(SESSION.count('if(Finished)return;'), 2)
        self.assertIn('time>=Map.Circles[^1].StartTime+3000', SESSION)

    def test_24_header_and_explicit_standard_mode(self):
        self.assertIn('version<5||version>14', PARSER)
        self.assertIn('mode!="0"', PARSER)

    def test_25_required_finite_difficulty(self):
        self.assertIn('!float.IsFinite(value)', PARSER)
        self.assertIn('value<0||value>10', PARSER)
        for name in ['CircleSize', 'OverallDifficulty', 'ApproachRate']:
            self.assertIn('Difficulty("' + name + '")', PARSER)

    def test_26_finite_coordinates_clamp_then_truncate(self):
        self.assertIn('Number(row[0],"circleX")', PARSER)
        self.assertIn('(int)Math.Clamp(x,0,512),(int)Math.Clamp(y,0,512)', PARSER)

    def test_27_type_and_object_time_validation(self):
        self.assertIn('(type&1)==0||(type&~(1|4|112))!=0', PARSER)
        self.assertIn('time<0||time>3_600_000', PARSER)

    def test_28_reject_empty_and_equal_time_maps(self):
        self.assertIn('if(circles.Count==0)throw', PARSER)
        self.assertIn('circles[i].StartTime==circles[i-1].StartTime', PARSER)

    def test_29_sort_readonly_and_size_limits(self):
        for term in ['circles.Sort((a,b)=>a.StartTime.CompareTo(b.StartTime))',
                     'circles.AsReadOnly()', 'Length>2*1024*1024', 'circles.Count>10000']:
            self.assertIn(term, PARSER)

    def test_30_QA01_D02_validate_required_hitsound_column(self):
        # This is the documented narrow-reader grammar, not a claim about the
        # original M09e5 forgiving/error policy, which is still unresolved.
        self.assertTrue('row[4]' in PARSER,
                        'QA01-D02: required fifth hit-object field is never read/validated; '
                        'malformed hitsound fixture is admitted by the reviewed source path.')

    def test_31_no_gui_source_linked_into_headless_suite(self):
        project = (HERE / 'Headless/QA01.Headless.csproj').read_text()
        for forbidden in ['Program.cs', 'PlayfieldView.cs', 'UseWPF', 'WindowsDesktop']:
            self.assertNotIn(forbidden, project)
        for required in ['Beatmap.cs', 'CircleSession.cs', 'PlayfieldLayout.cs']:
            self.assertIn(required, project)

    def test_32_bundled_fixture_identity(self):
        text = source('Fixtures/first-circles.osu')
        rows = text.split('[HitObjects]', 1)[1].strip().splitlines()
        self.assertEqual(len(rows), 8)
        self.assertEqual([int(r.split(',')[2]) for r in rows], list(range(3000, 13501, 1500)))
        self.assertIn('Mode: 0', text)


@unittest.skipUnless(os.environ.get('QA01_ARCHIVE'), 'pinned archive path not supplied')
class OriginalBytes(unittest.TestCase):
    @classmethod
    def setUpClass(cls):
        path = Path(os.environ['QA01_ARCHIVE'])
        cls.archive_hash = hashlib.sha256(path.read_bytes()).hexdigest()
        with zipfile.ZipFile(path) as z:
            cls.exe = z.read('osu!.exe')

    def test_33_archive_executable_hashes(self):
        self.assertEqual(self.archive_hash, PINS['archive_sha256'])
        self.assertEqual(hashlib.sha256(self.exe).hexdigest(), PINS['exe_sha256'])

    def test_34_selected_original_body_hashes(self):
        for method, r in PINS['original_methods'].items():
            with self.subTest(method=method):
                data = self.exe[r['file_offset']:r['file_offset'] + r['body_bytes']]
                self.assertEqual(hashlib.sha256(data).hexdigest(), r['body_sha256'])

    def test_35_strict_classification_original_bge(self):
        r = PINS['original_methods']['M1884']
        start = r['file_offset'] + r['header_bytes']
        for offset, target in [(0x25, 0x34), (0x40, 0x4f), (0x5b, 0x6a)]:
            self.assertEqual(self.exe[start + offset], 0x2f)  # bge.s
            rel = int.from_bytes(self.exe[start + offset + 1:start + offset + 2], 'little', signed=True)
            self.assertEqual(offset + 2 + rel, target)

    def test_36_ordered_spatial_original_ble(self):
        r = PINS['original_methods']['M24c7']
        start = r['file_offset'] + r['header_bytes']
        self.assertEqual(self.exe[start + 0x54:start + 0x56], bytes([0x31, 0x1a]))
        # ble.s 0070; distinct from unordered ble.un.s (0x36).

    def test_37_original_circle_timeout_equality_skip(self):
        r = PINS['original_methods']['M28e4']
        start = r['file_offset'] + r['header_bytes']
        self.assertEqual(self.exe[start + 0xbc:start + 0xbe], bytes([0x2f, 0x18]))
        # end + widest >= T -> 00d6 (skip), hence expiry requires strictly later.
