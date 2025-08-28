using System;

namespace RT.Coordinates;

/// <summary>Describes a directed edge going from one vertex to another.</summary>
public struct Edge(Vertex start, Vertex end) : IEquatable<Edge>
{
    /// <summary>The start vertex of the edge.</summary>
    public Vertex Start { get; private set; } = start;
    /// <summary>The end vertex of the edge.</summary>
    public Vertex End { get; private set; } = end;

    /// <inheritdoc/>
    public readonly bool Equals(Edge other) => other.Start.Equals(Start) && other.End.Equals(End);
    /// <inheritdoc/>
    public override readonly int GetHashCode() => unchecked(Start.GetHashCode() * -1521134295 + End.GetHashCode());
    /// <inheritdoc/>
    public override readonly bool Equals(object obj) => obj is Edge other && Equals(other);
    /// <summary>Converts this (directed) edge to an (undirected) link connecting the vertices.</summary>
    public readonly Link<Vertex> ToLink() => new(Start, End);

    /// <summary>Returns a new edge connecting the same vertices, but in the opposite direction.</summary>
    public readonly Edge Reverse => new(End, Start);

    /// <summary>Equality comparison operator.</summary>
    public static bool operator ==(Edge one, Edge two) => one.Equals(two);
    /// <summary>Inequality comparison operator.</summary>
    public static bool operator !=(Edge one, Edge two) => !one.Equals(two);

    /// <inheritdoc/>
    public override readonly string ToString() => $"{Start} → {End}";

    /// <summary>Deconstructor.</summary>
    public readonly void Deconstruct(out Vertex start, out Vertex end)
    {
        start = Start;
        end = End;
    }
}
