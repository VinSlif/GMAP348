using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBehavior : PlantBehavior {

	public PlantType type;

	public void OnDestroy() {
		switch(type) {
		case PlantType.Coca:
			// increase coca counter
			break;
		case PlantType.Kush:
			// increase kush counter
			break;
		case PlantType.Poppy:
			// increase poppy counter
			break;
		case PlantType.Psilocybin:
			// increase shroom counter
			break;
		}
	}
}