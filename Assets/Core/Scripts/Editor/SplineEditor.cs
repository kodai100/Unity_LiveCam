using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Text;

namespace kodai100.LiveCamCore.Splines
{
    [CustomEditor(typeof(Spline))]
    public class SplineEditor : Editor
    {
        public const float CIRCLE_IN_RAD = 2f * Mathf.PI;
        public const float SCALE_GUI = 0.05f;
        public const float SCALE_PICK = 0.07f;
        public const float SCALE_POS = 0.5f;

        public const int GIZMO_SMOOTH_LEVEL = 10;
        public const float NORMALIZE_ANGLE = 1f / 360;

        public const float JET_K_MIN = 0.01f;
        public const float JET_K_MAX = 0.1f;
        public const float JET_A = 0.66666f / (JET_K_MIN - JET_K_MAX);
        public const float JET_B = -JET_A * JET_K_MAX;

        private int _iSelectedCP = -1;
        private Tool _lastTool;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            var curve = (Spline) target;
            var cps = (curve.cps == null) ? (curve.cps = new Spline.ControlPoint[0]) : curve.cps;

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add"))
            {
                var size = 5f;
                var pos = (_iSelectedCP >= 0)
                    ? curve.Position(_iSelectedCP + 0.5f)
                    : (size * Random.onUnitSphere);
                var newCP = new Spline.ControlPoint() {position = pos};

                if (0 <= _iSelectedCP && _iSelectedCP < cps.Length)
                    ArrayUtil.Insert(ref cps, newCP, _iSelectedCP + 1);
                else
                    ArrayUtil.Insert(ref cps, newCP, cps.Length);
                curve.cps = cps;
                _iSelectedCP++;
                EditorUtility.SetDirty(curve);
            }

            if (GUILayout.Button("Remove"))
            {
                if (0 <= _iSelectedCP && _iSelectedCP < cps.Length)
                {
                    ArrayUtil.Remove(ref cps, _iSelectedCP);
                }

                curve.cps = cps;
                _iSelectedCP--;
                EditorUtility.SetDirty(curve);
            }

            EditorGUILayout.EndHorizontal();
        }

        void OnSceneGUI()
        {
            var curve = (Spline) target;
            var rot = Quaternion.identity;

            var cps = curve.cps;
            if (cps != null && cps.Length > 0)
            {
                for (var i = 0; i < cps.Length; i++)
                {
                    var cp = cps[i];
                    var cpPos = cp.position;
                    var size = HandleUtility.GetHandleSize(cpPos);
                    Handles.color = Color.white;
                    if (i == 0)
                        Handles.color = Color.green;
                    else if (i == (cps.Length - 1))
                        Handles.color = Color.red;
                    if (Handles.Button(cpPos, rot, size * SCALE_GUI, size * SCALE_PICK, Handles.DotHandleCap))
                    {
                        _iSelectedCP = i;
                        Repaint();
                    }
                }
            }

            Spline.ControlPoint selectedCP = null;
            if (0 <= _iSelectedCP && cps != null && _iSelectedCP < cps.Length)
                selectedCP = cps[_iSelectedCP];
            else
                _iSelectedCP = -1;

            if (selectedCP != null)
            {
                EditorGUI.BeginChangeCheck();
                var pos = Handles.DoPositionHandle(selectedCP.position, rot);
                if (EditorGUI.EndChangeCheck())
                {
                    selectedCP.position = pos;
                    EditorUtility.SetDirty(curve);
                }
            }
        }

        void OnEnable()
        {
            SceneView.onSceneGUIDelegate += DrawSceneGUI;
        }

        void OnDisable()
        {
            SceneView.onSceneGUIDelegate -= DrawSceneGUI;
        }

        void DrawSceneGUI(SceneView sceneView)
        {
            DrawGizmos();
            OnSceneGUI();
        }

        void DrawGizmos()
        {
            var spline = (Spline) target;
            var cps = spline.cps;
            System.Func<int, Vector3> GetCP = spline.GetCP;

            if (cps == null || cps.Length < 2)
                return;

            var dt = 1f / GIZMO_SMOOTH_LEVEL;
            var kMin = float.MaxValue;
            var kMax = 0f;
            for (var i = 0; i < cps.Length; i++)
            {
                var t = (float) i;
                for (var j = 0; j < GIZMO_SMOOTH_LEVEL; j++)
                {
                    var k = CatmullSplineUtil.Curvature(t, spline.GetCP);
                    k = Mathf.Clamp(k, JET_K_MIN, JET_K_MAX);
                    if (k < kMin)
                        kMin = k;
                    else if (kMax < k)
                        kMax = k;
                    t += dt;
                }
            }

            var jetA = 0.66666f / (kMin - kMax);
            var jetB = -jetA * kMax;
            var startPos = CatmullSplineUtil.Position(0f, GetCP);
            for (var i = 0; i < cps.Length; i++)
            {
                var t = (float) i;
                for (var j = 0; j < GIZMO_SMOOTH_LEVEL; j++)
                {
                    var k = CatmullSplineUtil.Curvature(t, spline.GetCP);
                    k = Mathf.Clamp(k, kMin, kMax);
                    Handles.color = Color.HSVToRGB(jetA * k + jetB, 1f, 1f);

                    var endPos = CatmullSplineUtil.Position(t += dt, GetCP);
                    Handles.DrawLine(startPos, endPos);
                    startPos = endPos;
                }
            }
        }

        [MenuItem("Assets/Create/Spline")]
        public static void CreateSpline()
        {
            ScriptableObjUtil.CreateAsset<Spline>();
        }
    }
}