using UnityEngine;
using System.Collections.Generic;

public class PolygonCollider2DGenerator : MonoBehaviour
{
	private int m_CurrentPathIndex = 0;
	private PolygonCollider2D m_PolygonCollider;
	private List<Edge> m_Edges = new List<Edge>();
	private List<Vector2> m_Points = new List<Vector2>();
	private Vector3[] m_Vertices;

	void Start()
	{
		// Get the polygon collider (create one if necessary)
		m_PolygonCollider = GetComponent<PolygonCollider2D>();
		if(m_PolygonCollider == null) {
			m_PolygonCollider = gameObject.AddComponent<PolygonCollider2D>();
		}

		// Get the mesh's vertices for use later
		m_Vertices = GetComponent<MeshFilter>().mesh.vertices;

		// Get all edges from triangles
		int[] triangles = GetComponent<MeshFilter>().mesh.triangles;
		for(int i = 0; i < triangles.Length; i += 3) {
			m_Edges.Add(new Edge(triangles[i], triangles[i + 1]));
			m_Edges.Add(new Edge(triangles[i + 1], triangles[i + 2]));
			m_Edges.Add(new Edge(triangles[i + 2], triangles[i]));
		}

		// Find duplicate edges
		List<Edge> edgesToRemove = new List<Edge>();
		foreach(Edge edge1 in m_Edges) {
			foreach(Edge edge2 in m_Edges) {
				if(edge1 != edge2) {
					if(edge1.vert1 == edge2.vert1 && edge1.vert2 == edge2.vert2 || edge1.vert1 == edge2.vert2 && edge1.vert2 == edge2.vert1) {
						edgesToRemove.Add(edge1);
					}
				}
			}
		}

		// Remove duplicate edges (leaving only perimeter edges)
		foreach(Edge edge in edgesToRemove) {
			m_Edges.Remove(edge);
		}

		// Start edge trace
		edgeTrace(m_Edges[0]);
	}

	void edgeTrace(Edge edge)
	{
		// Add this edge's vert1 coords to the point list
		m_Points.Add(m_Vertices[edge.vert1]);

		// Store this edge's vert2
		int vert2 = edge.vert2;

		// Remove this edge
		m_Edges.Remove(edge);

		// Find next edge that contains vert2
		foreach(Edge nextEdge in m_Edges) {
			if(nextEdge.vert1 == vert2) {
				edgeTrace(nextEdge);
				return;
			}
		}

		// No next edge found, create a path based on these points
		m_PolygonCollider.pathCount = m_CurrentPathIndex + 1;
		m_PolygonCollider.SetPath(m_CurrentPathIndex, m_Points.ToArray());

		// Empty path
		m_Points.Clear();

		// Increment path index
		m_CurrentPathIndex++;

		// Start next edge trace if there are edges left
		if(m_Edges.Count > 0) {
			edgeTrace(m_Edges[0]);
		}
	}
}

class Edge
{
	public int vert1;
	public int vert2;

	public Edge(int Vert1, int Vert2)
	{
		vert1 = Vert1;
		vert2 = Vert2;
	}
}