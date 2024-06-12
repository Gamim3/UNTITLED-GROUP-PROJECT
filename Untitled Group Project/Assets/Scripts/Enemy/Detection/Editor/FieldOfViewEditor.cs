using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Enemy))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        Enemy fov = (Enemy)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radius);

        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle01 * fov.radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle02 * fov.radius);

        Vector3 throwAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.throwAngle / 2);
        Vector3 throwAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.throwAngle / 1.8f);

        Handles.color = Color.magenta;
        Handles.DrawLine(fov.transform.position, fov.transform.position + throwAngle01 * fov.radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + throwAngle02 * fov.radius);

        if (fov.playerInSight)
        {
            Handles.color = Color.red;
            Handles.DrawLine(fov.transform.position, fov.player.transform.position);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
