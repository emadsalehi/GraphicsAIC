using UnityEngine;

public class AttackEffectController : MonoBehaviour
{
    public GameObject particleSystemGameObject;

    private Vector3 _destination;
    private GameObject _target;
    private ParticleSystem particleSystem;
    private bool _attackEnable = false;

    void Start()
    {   
        particleSystem = particleSystemGameObject.GetComponent<ParticleSystem>();
        if (_target != null) {
            _destination = _target.transform.position;
        }
    }
  

    public void StopParticleSystem() {
        if (particleSystem == null) return;
        particleSystem.Stop();
        _attackEnable = false;
    }

    public void PlayParticleSystem(GameObject target)
    {
        _target = target;
        if (_target == null) return;
        _destination = transform.position - particleSystemGameObject.transform.position;
        particleSystem.Play();
        _attackEnable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_attackEnable) {
            particleSystemGameObject.transform.LookAt(_target.GetComponent<Details>().offset + _target.transform.position);
        }
    }
}
