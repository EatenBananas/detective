using UnityEngine;
using static System.Guid;

namespace Scenes.SaveSystem.SecondSaveSystem
{
    public abstract class SavableBehaviour : MonoBehaviour
    {
        [field: SerializeField] public virtual string Key { get; private set; }
        public abstract object DefaultValue { get; }
        
        #region Unity Methods

        public virtual void OnValidate()
        {
            if (string.IsNullOrEmpty(Key))
                Key = NewGuid().ToString();
        }

        public virtual void Awake() =>
            Register();

        public virtual void OnDestroy() => 
            Unregister();

        #endregion
        
        private void Register() => 
            SaveManager.SavableBehaviours.Add(this);
        
        private void Unregister() =>
            SaveManager.SavableBehaviours.Remove(this);

        public abstract object GetValue();
        public abstract void SetValue(object value);
        
        public virtual void UpdateValue()
        {
            SecondSaveSystem.Save.SetKey(Key, GetValue());
        }
    }
}