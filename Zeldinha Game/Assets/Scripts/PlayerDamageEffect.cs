using UnityEngine;
using UnityEngine.Rendering;

public class PlayerDamageEffect : MonoBehaviour
{
	[SerializeField] private Volume m_volume;
    [SerializeField] private float minWeight = 0.5f;
    [SerializeField] private float maxWeight = 1.0f;
    private Life lifeScript;

    private void Awake()
    {
        lifeScript = GetComponent<Life>();
    }

    private void Start()
    {
        lifeScript.OnDamage += OnDamage;
    }

    private void Update()
    {
       m_volume.weight = Mathf.Lerp(m_volume.weight, 0, Time.deltaTime);
    }

    private void OnDamage(object sender, DamageEventArgs e)
    {
        float lifeRate = lifeScript.GetHealthRate();

        m_volume.weight = minWeight + (maxWeight - minWeight) * (1-lifeRate);
    }
}
