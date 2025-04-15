using DG.Tweening;
using Enums;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Managers {
    public class OxygenManager : MonoBehaviour, IManager {

    #region Dependencies
        [Header("Variables")]
        [SerializeField] private float decayRate = 0.005f;
        [SerializeField] private float growRate = 0.005f;
        [SerializeField] private float jetpackDecayRate = 0.01f;
        [SerializeField] private float jetpackInitialDrain = 0.01f;

        [Header("UI")] 
        [SerializeField] private Image barUnlockImage;
        [SerializeField] private Image oxygenImage, quadrantsImage;
        [SerializeField] private Image faceImage;
        [SerializeField] private Sprite happyFaceSprite, sadFaceSprite;
        [SerializeField] private Volume volume;
    #endregion

    #region Attributes
        public static OxygenManager Instance { get; private set; }
        public float OxygenPercent {
            get => _oxygenPercent;
            set {
                _oxygenPercent = value;
            
                var maxAmount = _upgrades switch {
                    0 => 0.5f,
                    1 => 0.75f,
                    2 => 1f,
                    _ => 1f
                };

                oxygenImage.fillAmount = maxAmount * _oxygenPercent;
            }
        }
    
        public int Upgrades {
            get => _upgrades;
            set {
                _upgrades = value;

                quadrantsImage.fillAmount = _upgrades switch {
                    0 => 0.7f,
                    1 => 0.4f,
                    2 => 0.1f,
                    _ => 0.1f
                };
            
                var amount = _upgrades switch {
                    0 => 0.5f,
                    1 => 0.75f,
                    2 => 1f,
                    _ => 1f
                };

                oxygenImage.DOFade(0f, 0f);
                DOVirtual.Float(barUnlockImage.fillAmount, amount, 1f, f => barUnlockImage.fillAmount = f);
                oxygenImage.DOFade(1f, 0.2f).SetDelay(1f);
                
                oxygenImage.fillAmount = amount * _oxygenPercent;
            }
        }
    #endregion

    #region Data
        private float _oxygenPercent = 1f;
        private int _upgrades;
        private bool _outOfBubble;
        private bool _jetpackOn;
        private float _startingVignetteIntensity;
        private Tweener _effectTweenerOne, _effectTweenerTwo;
    #endregion

    #region Unity
        private void Awake() {
            Instance = this;
            volume.profile.TryGet(typeof(Vignette), out Vignette vignette);
            _startingVignetteIntensity = vignette.intensity.value;
        }
        
        private void FixedUpdate() {
            if (_outOfBubble) {
                AddOxygen(-decayRate);
            } else {
                AddOxygen(growRate);
            }
        
            if (_jetpackOn) {
                AddOxygen(-jetpackDecayRate);
            }
        }
    #endregion

    #region Custom
        public void Init() {
            enabled = false;
            Upgrades = SaveManager.Instance.NumberOfItem(ItemType.OxygenUpgrade);
            OxygenPercent = 1f;
            OutOfBubble();
            TitleEffects();
        }

        public void Upgrade(ItemId itemId) {
            Upgrades++;
        }
        
        public void SceneChange(string sceneName) {}
    
        public void ToggleJetpack(bool b) {
            _jetpackOn = b;
        
            if (b) {
                AddOxygen(-jetpackInitialDrain);
            }
        }
        
        public void Hurt(int damage) {
            faceImage.sprite = sadFaceSprite;
            DOVirtual.DelayedCall(0.7f, () => faceImage.sprite = happyFaceSprite);
            
            AddOxygen(-damage/100f);
        }

        private void AddOxygen(float amount) {
            var trueRate = amount > 0f ? amount * (_upgrades * 0.3f + 1) : amount / (_upgrades * 0.5f + 1);
            const float lowAirThreshold = 0.25f;
        
            OxygenPercent += trueRate;

            if (OxygenPercent <= 0f) {
                OxygenPercent = 0f;
                
                GameManager.Instance.GameOver();
            } else if (OxygenPercent <= lowAirThreshold && !AudioManager.Instance.PlayingLowAirTheme) {
                GrowEffects();
                AudioManager.Instance.PlayLowAirTheme();
            } else if (OxygenPercent >= 1f) {
                OxygenPercent = 1f;
            } else if (OxygenPercent > lowAirThreshold && AudioManager.Instance.PlayingLowAirTheme) {
                ShrinkEffects();
                AudioManager.Instance.PlayGameTheme();
            }
        }

        public void ResetVolume() {
            ShrinkEffects();
        }

        private void GrowEffects() {
            _effectTweenerOne?.Kill();
            _effectTweenerTwo?.Kill();
            
            if (volume.profile.TryGet(typeof(Vignette), out Vignette vignette)) {
                _effectTweenerOne = DOVirtual.Float(vignette.intensity.value, _startingVignetteIntensity * 2f,  1f, (f) => vignette.intensity.value = f);
            }
            if (volume.profile.TryGet(typeof(ChromaticAberration), out ChromaticAberration chromaticAberration)) {
                _effectTweenerTwo = DOVirtual.Float(chromaticAberration.intensity.value, 1f,  1f, (f) => chromaticAberration.intensity.value = f);
            }
        }
        
        public void ShrinkEffects() {
            _effectTweenerOne?.Kill();
            _effectTweenerTwo?.Kill();
            
            if (volume.profile.TryGet(typeof(Vignette), out Vignette vignette)) {
                _effectTweenerOne = DOVirtual.Float(vignette.intensity.value, _startingVignetteIntensity,  0.5f, (f) => vignette.intensity.value = f);
            }
            if (volume.profile.TryGet(typeof(ChromaticAberration), out ChromaticAberration chromaticAberration)) {
                _effectTweenerTwo = DOVirtual.Float(chromaticAberration.intensity.value, 0f,  1f, (f) => chromaticAberration.intensity.value = f);
            }
        }

        private void TitleEffects() {
            _effectTweenerOne?.Kill();
            _effectTweenerTwo?.Kill();

            if (volume.profile.TryGet(typeof(Vignette), out Vignette vignette)) {
                _effectTweenerOne = DOVirtual.Float(vignette.intensity.value, 0.5f,  0f, (f) => vignette.intensity.value = f);
            }
            if (volume.profile.TryGet(typeof(ChromaticAberration), out ChromaticAberration chromaticAberration)) {
                _effectTweenerTwo = DOVirtual.Float(chromaticAberration.intensity.value, 0f,  0f, (f) => chromaticAberration.intensity.value = f);
            }
        }

        public void OutOfBubble() {
            _outOfBubble = true;
        }
    
        public void InBubble() {
            _outOfBubble = false;
        }
    
    #endregion

    }
}