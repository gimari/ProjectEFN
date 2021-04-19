using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EFN {

	public enum eDealerType {
		None = 0,
		Prapor = 1,
		Therapist = 2,
		Fence = 3,
	}

	public class Status_Dealer {

		private static Dictionary<eDealerType, Status_Dealer> _statusList = new Dictionary<eDealerType, Status_Dealer>();

		public static Status_Dealer GetStatus(eDealerType dealer) {

			if (false == _statusList.ContainsKey(dealer)) {
				Status_Dealer status;

				switch (dealer) {
					case eDealerType.Prapor:
						status = new Status_Prapor();
						break;

					case eDealerType.Therapist:
						status = new Status_Therapist();
						break;

					case eDealerType.Fence:
						status = new Status_Fence();
						break;

					default:
						status = null;
						break;
				}

				_statusList.Add(dealer, status);
			}

			return _statusList[dealer];
		}

		private Inventory_Item _dealerInven = new Inventory_Item();
		public Inventory_Item DealerInven { get { return this._dealerInven; } }

		public Status_Dealer() {
			RandomDealerInven();
		}

		protected void SetDealerInven(List<KeyValuePair<eItemType, int>> table) {
			_dealerInven.MaxDisplayIndex = table.Count;

			foreach (KeyValuePair<eItemType, int> pair in table) {
				Data_Item item = new Data_Item(pair.Key);
				item.StackCount = pair.Value;

				_dealerInven.AddInventory(item);
			}
		}

		/// <summary>
		/// 딜러의 인벤토리를 사전에 정의된 값으로 채워줌.
		/// </summary>
		protected virtual void RandomDealerInven() { }

		/// <summary>
		/// 특정 아이템에 대한 딜러의 기본 지불값 반환
		/// </summary>
		public virtual long GetDefaultCost(eItemType item) { return 10; }
	}

	public class Status_Prapor : Status_Dealer {

		protected override void RandomDealerInven() {
			base.RandomDealerInven();

			List<KeyValuePair<eItemType, int>> itemDropTable = new List<KeyValuePair<eItemType, int>>();

			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.AMMO_9X19AP, 1));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.AMMO_9X19AP, 15));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.AMMO_9X19AP, 30));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.AMMO_9X19AP, 90));

			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.Weapon_MP443, 1));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.WEAPON_MP5, 1));

			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.AMMO_762BP, 1));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.AMMO_762BP, 15));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.AMMO_762BP, 30));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.AMMO_762BP, 90));

			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.WEAPON_AKM, 1));

			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.AMMO_23X75, 1));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.AMMO_23X75, 5));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.AMMO_23X75, 9));

			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.WEAPON_KS23M, 1));

			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.AMMO_9X39SP5, 1));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.AMMO_9X39SP5, 15));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.AMMO_9X39SP5, 30));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.AMMO_9X39SP5, 90));

			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.AMMO_556M855, 1));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.AMMO_556M855, 15));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.AMMO_556M855, 30));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.AMMO_556M855, 90));

			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.AMMO_45AP, 1));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.AMMO_45AP, 20));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.AMMO_45AP, 40));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.AMMO_45AP, 120));

			SetDealerInven(itemDropTable);
		}

		public override long GetDefaultCost(eItemType item) {
			switch (item) {
				case eItemType.Weapon_MP443:	return 5230;
				case eItemType.WEAPON_ASVAL:	return 46983;
				case eItemType.WEAPON_AKM:		return 7248;
				case eItemType.WEAPON_HK416:	return 51376;
				case eItemType.WEAPON_M1911:	return 7623;
				case eItemType.WEAPON_KS23M: return 13487;
				case eItemType.WEAPON_MP5: return 8291;
				case eItemType.WEAPON_SKS: return 42152;
				case eItemType.WEAPON_VECTOR: return 33842;

				case eItemType.CONS_FIRSTAID:	return 36;

				case eItemType.NONE_COKE0: return 100;
				case eItemType.NONE_COKE1: return 600;
				case eItemType.NONE_COKE2: return 900;
				case eItemType.NONE_COKE3: return 1200;
				case eItemType.NONE_COKE4: return 1800;
				case eItemType.NONE_COKE5: return 2400;
				case eItemType.NONE_COKE6: return 3000;
				case eItemType.NONE_COKE7: return 3600;
				case eItemType.NONE_COKE8: return 4200;
				case eItemType.NONE_COKE9: return 4800;
				case eItemType.NONE_COKE10: return 5400;
				case eItemType.NONE_COKE11: return 6000;
				case eItemType.NONE_COKE12: return 6600;
				case eItemType.NONE_COKE13: return 7200;
				case eItemType.NONE_COKE14: return 7800;

				case eItemType.NONE_TROPHY0: return 15468;
				case eItemType.NONE_TROPHY1: return 7625;
				case eItemType.NONE_TROPHY2: return 6728;
				case eItemType.NONE_TROPHY3: return 11926;
				case eItemType.NONE_TROPHY4: return 34598;
				case eItemType.NONE_TROPHY5: return 444444;

				default: return base.GetDefaultCost(item);
			}
		}
	}

	public class Status_Therapist : Status_Dealer {

		protected override void RandomDealerInven() {
			base.RandomDealerInven();

			List<KeyValuePair<eItemType, int>> itemDropTable = new List<KeyValuePair<eItemType, int>>();

			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.CONS_FIRSTAID, 1));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.CONS_FIRSTAID, 3));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.WEAPON_RECORDER, 1));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.WEAPON_DAGGER, 1));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.HEAD_STAR, 1));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.HEAD_RABBIT, 1));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.HEAD_DDU, 1));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.HEAD_FEDORA, 1));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.HEAD_COLLET, 1));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.HEAD_TEM, 1));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.HEAD_YOOWOO, 1));

			SetDealerInven(itemDropTable);
		}

		public override long GetDefaultCost(eItemType item) {
			switch (item) {
				case eItemType.CONS_FIRSTAID: return 360;

				case eItemType.NONE_COKE0:	return 100;
				case eItemType.NONE_COKE1:	return 600;
				case eItemType.NONE_COKE2:	return 900;
				case eItemType.NONE_COKE3:	return 1200;
				case eItemType.NONE_COKE4:	return 1800;
				case eItemType.NONE_COKE5:	return 2400;
				case eItemType.NONE_COKE6:	return 3000;
				case eItemType.NONE_COKE7:	return 3600;
				case eItemType.NONE_COKE8:	return 4200;
				case eItemType.NONE_COKE9:	return 4800;
				case eItemType.NONE_COKE10: return 5400;
				case eItemType.NONE_COKE11: return 6000;
				case eItemType.NONE_COKE12: return 6600;
				case eItemType.NONE_COKE13: return 7200;
				case eItemType.NONE_COKE14: return 7800;

				case eItemType.NONE_TROPHY0: return 15468;
				case eItemType.NONE_TROPHY1: return 7625;
				case eItemType.NONE_TROPHY2: return 6728;
				case eItemType.NONE_TROPHY3: return 11926;
				case eItemType.NONE_TROPHY4: return 34598;
				case eItemType.NONE_TROPHY5: return 444444;

				case eItemType.HEAD_COLLET: return 30080;
				case eItemType.HEAD_DDU: return 36052;
				case eItemType.HEAD_FEDORA: return 35080;
				case eItemType.HEAD_RABBIT: return 31016;
				case eItemType.HEAD_STAR: return 4444;
				case eItemType.HEAD_TEM: return 31350;
				case eItemType.HEAD_YOOWOO: return 33353;

				default: return base.GetDefaultCost(item);
			}
		}
	}

	public class Status_Fence : Status_Dealer {
	}
}