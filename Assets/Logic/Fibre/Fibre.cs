using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fibre {
    private List<Point4D> fibrePoints;

    public Fibre(List<Point4D> fibrePoints) {
        this.fibrePoints = fibrePoints;
    }
    public List<Point3D> project() {
        List<Point3D> projectedFibre = new List<Point3D>();
        foreach (Point4D point in fibrePoints) {
            projectedFibre.Add(point.project());
        }
        return projectedFibre;
    }
}
