using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Cube : MonoBehaviour {

    public int xSize, ySize, zSize;

    private Vector3[] vertices;

    private Mesh mesh;

    private void Awake () {
        StartCoroutine(Generate());
    }

    private IEnumerator Generate () {
        WaitForSeconds wait = new WaitForSeconds(0.01f);

        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Cube";

        int cornerVertices = 8;
        int edgeVertices = (xSize + ySize + zSize - 3) * 4;
        int faceVertices = (
            (xSize - 1) * (ySize - 1) +
            (xSize - 1) * (zSize - 1) +
            (ySize - 1) * (zSize - 1)) * 2;
        vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];

        int v = 0;
        Debug.Log("vertices.Length " + vertices.Length);
        // one layer at a time
        for (int y = 0; y <= ySize; y++) {
            // outer faces and edges
            for (int x = 0; x <= xSize; x++) {
                vertices[v++] = new Vector3(x, y, 0);
                yield return wait;
            }
            for (int z = 1; z <= zSize; z++) {
                vertices[v++] = new Vector3(xSize, y, z);
                yield return wait;
            }
            for (int x = xSize - 1; x >= 0; x--) {
                vertices[v++] = new Vector3(x, y, zSize);
                yield return wait;
            }
            for (int z = zSize - 1; z > 0; z--) {
                vertices[v++] = new Vector3(0, y, z);
                yield return wait;
            }
        }
        // top face fill
        for (int z = 1; z < zSize; z++) {
            for (int x = 1; x < xSize; x++) {
                vertices[v++] = new Vector3(x, ySize, z);
                yield return wait;
            }
        }
        // bottom face fill
        for (int z = 1; z < zSize; z++) {
            for (int x = 1; x < xSize; x++) {
                vertices[v++] = new Vector3(x, 0, z);
                yield return wait;
            }
        }
    }

    private void OnDrawGizmos () {
        if (vertices == null) {
            return;
        }

        Gizmos.color = Color.black;
        foreach (Vector3 vertice in vertices) {
            Gizmos.DrawSphere(vertice, 0.1f);
        }
    }
}
