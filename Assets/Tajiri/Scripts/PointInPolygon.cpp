extern "C" __declspec(dellexport)

bool IsPointInPolygon(float px, float py, float* polygon, int count)
{
    bool inside = false;
    int j = count - 1;

    for (int i = 0; i < count; i++) {
        float xi = polygon[i * 2];
        float yi = polygon[i * 2 + 1];
        float xj = polygon[j * 2];
        float yj = polygon[j * 2 + 1];

        if (((yi > py) != (yj > py)) && (px < (xj - xi) * (py - yi) / (yj - yi + 1e-7) + xi)) {
           inside = !inside; 
        }
        j = i;
    }

    return inside;
}
