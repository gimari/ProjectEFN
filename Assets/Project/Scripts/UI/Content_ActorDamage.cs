using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace EFN.Game {
	public class Content_ActorDamage : MonoBehaviour {

		[SerializeField] private Animator _animator = default;
		[SerializeField] private Text _txtDmg = default;

		public void PlayDamage(float dmg) {
			_txtDmg.text = dmg.ToString();
			// _animator.Play("Fire");
			_txtDmg.rectTransform.DOAnchorPosX(Random.Range(-100, 100), 0.5f);

			_txtDmg.rectTransform.DOAnchorPosY(200, 0.1f).SetEase(Ease.OutCirc).OnComplete(() => {
				_txtDmg.rectTransform.DOAnchorPosY(Random.Range(0, 100), 0.3f).SetEase(Ease.InCirc);
			});

			_txtDmg.rectTransform.DOScale(0, 0.5f);
			_txtDmg.DOFade(0, 0.5f);

			StartCoroutine(DamagePlayRoutine());
		}

		private IEnumerator DamagePlayRoutine() {
			yield return new WaitForSeconds(1f);

			Destroy(this.gameObject);
		}

	}
}