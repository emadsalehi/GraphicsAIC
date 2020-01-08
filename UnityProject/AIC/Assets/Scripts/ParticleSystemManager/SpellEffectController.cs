using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffectController : MonoBehaviour
{
    public GameObject Haste;
    public GameObject Damage;
    public GameObject Heal;
    public GameObject Teleport;
    public GameObject Duplicate;
    public GameObject Poison;

    public void StartSpell(int typeId)
    {
        switch (typeId)
        {
            case 0:
                Haste.GetComponent<ParticleSystem>().Play();
                break;
            case 1:
                Damage.GetComponent<ParticleSystem>().Play();
                break;
            case 2:
                Heal.GetComponent<ParticleSystem>().Play();
                break;
            case 3:
                Teleport.GetComponent<ParticleSystem>().Play();
                break;
            case 4:
                Duplicate.GetComponent<ParticleSystem>().Play();
                break;
            case 5:
                Poison.GetComponent<ParticleSystem>().Play();
                break;
        }
    }
    
    public void StopSpell(int typeId)
    {
        switch (typeId)
        {
            case 0:
                Haste.GetComponent<ParticleSystem>().Stop();
                break;
            case 1:
                Damage.GetComponent<ParticleSystem>().Stop();
                break;
            case 2:
                Heal.GetComponent<ParticleSystem>().Stop();
                break;
            case 3:
                Teleport.GetComponent<ParticleSystem>().Stop();
                break;
            case 4:
                Duplicate.GetComponent<ParticleSystem>().Stop();
                break;
            case 5:
                Poison.GetComponent<ParticleSystem>().Stop();
                break;
        }
    }

    public void StopAll()
    {
        Haste.GetComponent<ParticleSystem>().Stop(true);
        Damage.GetComponent<ParticleSystem>().Stop(true);
        Heal.GetComponent<ParticleSystem>().Stop(true);
        Teleport.GetComponent<ParticleSystem>().Stop(true);
        Duplicate.GetComponent<ParticleSystem>().Stop(true);
        Poison.GetComponent<ParticleSystem>().Stop(true);
    }
}
