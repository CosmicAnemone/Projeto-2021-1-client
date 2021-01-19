using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;

namespace ui {
	public class NavButton : MonoBehaviour, IPointerClickHandler {
		public GameObject from, to;

		public void OnPointerClick(PointerEventData eventData) {
			from.SetActive(false);
			to.SetActive(true);
		}
	}
}
