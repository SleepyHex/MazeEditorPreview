using UnityEngine;

public class BreadCrumb
{
    Material mat;
    MaterialPropertyBlock propBlock;
    MeshRenderer meshRend;
    readonly int shPropColor = Shader.PropertyToID("_Color");

    void ApplyColorNew(Renderer rend, Color color)
    {
        propBlock.SetColor(shPropColor, color);
        rend.SetPropertyBlock(propBlock);
    }

    public Transform PlaceBreadcrumb(Vector3 worldPos, Color color, float Size)
    {
        var breadCrumb = GameObject.CreatePrimitive(PrimitiveType.Quad);

        if(meshRend == null)
            meshRend = breadCrumb.GetComponent<MeshRenderer>();

        if(propBlock == null)
            propBlock = new();


        breadCrumb.name = "BreadCrumb";
        breadCrumb.transform.localScale = Vector3.one * Size;
        breadCrumb.transform.rotation = Quaternion.Euler(90, 0, 0);
        breadCrumb.transform.position = worldPos + Vector3.up * 0.01f;
        ApplyColorNew(breadCrumb.GetComponent<Renderer>(), color);

        return breadCrumb.transform;
    }
}
