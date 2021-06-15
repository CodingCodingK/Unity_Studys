using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EF_Waterwave : MonoBehaviour
{
    public Texture[] textures;// 导入的水纹材质列表
    private Material material;// 材质
    private int index;

    void Start()
    {
	    material = this.GetComponent<MeshRenderer>().material;
	    InvokeRepeating("TextureSlide", 0, 0.1f);
    }

    private void TextureSlide()
    {
	    material.mainTexture = textures[index];
	    index = (index+1) % textures.Length;
    }
}
