using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField]
    GameObject doubleRingMarker;
    public GameObject DoubleRingMarker { get { return doubleRingMarker; } }
    public static VFXManager instance;

    [SerializeField]
    private GameObject[] magicVFX;
    public GameObject[] MagicVFX { get { return magicVFX; } }

    [SerializeField]
    private MagicData[] magicData;
    public MagicData[] MagicData { get { return magicData; } }

    void Awake()
    {
        instance = this;
    }

    public void LoadMagic(int id, Vector3 posA, float time)
    {
        if (magicVFX[id] == null)
            return;
        GameObject objLoad = Instantiate(magicVFX[id], posA, Quaternion.identity);
        Destroy(objLoad, time);
    }
    public void ShootMagic(int id, Vector3 posA, Vector3 posB, float time)
    {
        if (magicVFX[id] == null)
            return;

        GameObject objShoot = Instantiate(magicVFX[id], posA, Quaternion.identity);
        objShoot.transform.position = Vector3.LerpUnclamped(posA, posB, time);
        Destroy(objShoot, time);

        Debug.Log("1");
    }

    private void OnEnable()
    {
        MyAction.onLoadMagic += LoadMagic;
        MyAction.onShootMagic += ShootMagic;
        MyAction.onCreateMagic += CreateMagic;  // === 47.14 เพิ่ม ===
    }

    private void OnDisable()
    {
        MyAction.onLoadMagic -= LoadMagic;
        MyAction.onShootMagic -= ShootMagic;
        MyAction.onCreateMagic -= CreateMagic;  // === 47.14 เพิ่ม ===
    }

    public Magic CreateMagic(int id)
    {
        return new Magic(magicData[id]);
    }
}
