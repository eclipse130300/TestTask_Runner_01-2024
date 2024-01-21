using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class ActorUI : MonoBehaviour
    {
        [SerializeField]
        private HpBar _hpBar;

        private IHealth _heroHealth;
    
        private void Start()
        {
            IHealth health = GetComponent<IHealth>();
      
            if(health != null)
                Construct(health);
        }

        public void Construct(IHealth heroHealth)
        {
            _heroHealth = heroHealth;
            _heroHealth.OnHealthChanged += UpdateHpBar;
        }

        private void OnDestroy() => 
            _heroHealth.OnHealthChanged -= UpdateHpBar;

        private void UpdateHpBar() => 
            _hpBar.SetValue(_heroHealth.Current, _heroHealth.Max);
    }
}