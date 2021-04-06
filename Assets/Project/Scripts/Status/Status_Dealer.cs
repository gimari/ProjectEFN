﻿using System.Collections;
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
	}

	public class Status_Prapor : Status_Dealer {
	}

	public class Status_Therapist : Status_Dealer {
	}

	public class Status_Fence : Status_Dealer {
	}
}