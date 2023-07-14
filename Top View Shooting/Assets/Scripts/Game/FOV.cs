using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour
{
    [SerializeField]
    private int count;
    [SerializeField]
    private float dist;
    [SerializeField]
    private float angle;

    [SerializeField]
    private MeshFilter meshFilter;
    [SerializeField]
    private Transform player;

    void Update()
    {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[count + 1];
        int[] indexes = new int[(count - 1) * 3];

        Vector3 lookDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - player.position;
        float look = Mathf.Atan2(lookDir.y, lookDir.x);

        for (int i = 0; i < count; i++)
        {
            float theta = look - (angle * Mathf.Deg2Rad) / 2 + (angle * Mathf.Deg2Rad) / (count - 1) * i;
            Vector3 dir = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta));
            Vector3 start = player.position + dir * 1.5f;
            RaycastHit2D hit = Physics2D.Raycast(start, dir, dist);

            if (hit.collider != null)
                vertices[i] = new Vector3(hit.point.x, hit.point.y);
            else
                vertices[i] = start + dir * dist;
            vertices[i].z = -8f;

            if (i > 0)
            {
                indexes[(i - 1) * 3] = count;
                indexes[(i - 1) * 3 + 1] = i;
                indexes[(i - 1) * 3 + 2] = i - 1;
            }
        }

        vertices[count] = player.position;
        Debug.Log((vertices[0], vertices[1], vertices[2]));

        mesh.vertices = vertices;
        mesh.triangles = indexes;
        meshFilter.mesh = mesh;
    }
}
