#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace EnemySystem
{
    [CustomEditor(typeof(ViewArea))]
    public class ViewAreaEditor : Editor
    {
        private void OnSceneGUI()
        {
            var script = (ViewArea)target;
            if (script.EnemyEye == null) return;
        
            Handles.color = Color.red;
            Handles.DrawWireArc(script.EnemyEye.position, Vector3.up, Vector3.forward, 360, script.Radius);

            var viewAngle1 = DirectionFromAngle(script.EnemyEye.eulerAngles.y, -script.Angle / 2);
            var viewAngle2 = DirectionFromAngle(script.EnemyEye.eulerAngles.y, script.Angle / 2);

            Handles.color = Color.yellow;
            Handles.DrawLine(script.EnemyEye.position, script.EnemyEye.position + viewAngle1 * script.Radius);
            Handles.DrawLine(script.EnemyEye.position, script.EnemyEye.position + viewAngle2 * script.Radius);

            if (script.CanSeeVictim)
            {
                Handles.color = Color.green;
                foreach (var victim in script.VictimsInArea)
                    Handles.DrawLine(script.EnemyEye.position, victim.transform.position);
            }
        }

        private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
        {
            angleInDegrees += eulerY;
            return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
        }
    }
}
#endif
