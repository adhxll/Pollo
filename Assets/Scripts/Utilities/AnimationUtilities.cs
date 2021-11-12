using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; 

public class AnimationUtilities : MonoBehaviour
{
    //Taro gameobject yang mau dianimate sebagai parameter, trus jalanin. 
    //Pastiin kalo mau animate terus objectnya dihilangkan / mau pindah scene, jalanin func animate baru 
    //jalanin fungsi pindah/hilang pake invoke
    public static void AnimateButtonPush(GameObject obj) { //animate squish seperti pencet tombol
        obj.transform.DORewind();
          
        float vectorX = obj.transform.localScale.x * -0.25f;
        float vectorY = obj.transform.localScale.y * -0.25f;
        float vectorZ = obj.transform.localScale.z * -0.25f; 

        obj.transform.DOPunchScale(new Vector3(vectorX, vectorY, vectorZ), 0.2f, 1, 1);
    }
    public static void AnimatePopUp(GameObject obj) { //animate expand kebalikan dari pencet tombol
        obj.transform.DORewind();
      
        float vectorX = obj.transform.localScale.x * 0.25f;
        float vectorY = obj.transform.localScale.y * 0.25f;
        float vectorZ = obj.transform.localScale.z * 0.25f; 
       
        obj.transform.DOPunchScale(new Vector3(vectorX, vectorY, vectorZ), 0.3f, 1);
    }
    public static void AnimatePopUpDisappear(GameObject obj) { //animate shrinking menjadi super kecil (belom menghilang)
        obj.transform.DORewind();

        float vectorX = obj.transform.localScale.x * -0.99f;
        float vectorY = obj.transform.localScale.y * -0.99f;
        float vectorZ = obj.transform.localScale.z * -0.99f;

        obj.transform.DOPunchScale(new Vector3(vectorX, vectorY, vectorZ), 0.6f, 1);
    }

}
