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
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.AMMO_9X19AP, 5));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.AMMO_9X19AP, 11));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.Weapon_MP443, 1));

			SetDealerInven(itemDropTable);
		}

		public override long GetDefaultCost(eItemType item) {
			switch (item) {
				case eItemType.Armor_6B3TM:		return 6732;
				case eItemType.Weapon_MP443:	return 823;
				case eItemType.AMMO_9X19AP:		return 1;
				case eItemType.WEAPON_ASVAL:	return 1023;
				case eItemType.AMMO_9X39SP5:	return 2;
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
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.CONS_FIRSTAID, 1));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.WEAPON_RECORDER, 1));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.HEAD_STAR, 1));
			itemDropTable.Add(new KeyValuePair<eItemType, int>(eItemType.HEAD_RABBIT, 1));

			SetDealerInven(itemDropTable);
		}

		public override long GetDefaultCost(eItemType item) {
			switch (item) {
				case eItemType.Armor_6B3TM: return 1159;
				case eItemType.CONS_FIRSTAID: return 360;
				case eItemType.HEAD_STAR: return 4444;
				case eItemType.WEAPON_RECORDER: return 4444;

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

				default: return base.GetDefaultCost(item);
			}
		}
	}

	public class Status_Fence : Status_Dealer {
	}
}