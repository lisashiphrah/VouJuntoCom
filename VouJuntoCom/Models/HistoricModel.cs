using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VouJuntoCom.Models
{
	public class HistoricModel
	{
		public HistoricModel()
		{
			this.InicializeLucroAnualOferecidas();
			this.InicializeLucroAnualAceitas();
			this.InicializeCaronasAnuaisOferecidas();
			this.InicializeCaronasAnuaisAceitas();
		}

		public List<RidesModel> OfferedRides { get; set; }
		public List<RidesModel> ReceivedRides { get; set; }
		public double? DrivedDistance { get; set; }
		public double? AcceptedDistance { get; set; }
		public double TotalGain { get; set; }
		public double TotalPaid { get; set; }
		public Dictionary<int, double> LucroAnualOferecidas { get; set; }
		public Dictionary<int, double> LucroAnualAceitas { get; set; }
		public Dictionary<int, int> CaronasAnuaisOferecidas { get; set; }
		public Dictionary<int, int> CaronasAnuaisAceitas { get; set; }

		private void InicializeCaronasAnuaisAceitas()
		{
			this.CaronasAnuaisAceitas = new Dictionary<int, int>();
			this.CaronasAnuaisAceitas.Add(1, 0);
			this.CaronasAnuaisAceitas.Add(2, 0);
			this.CaronasAnuaisAceitas.Add(3, 0);
			this.CaronasAnuaisAceitas.Add(4, 0);
			this.CaronasAnuaisAceitas.Add(5, 0);
			this.CaronasAnuaisAceitas.Add(6, 0);
			this.CaronasAnuaisAceitas.Add(7, 0);
			this.CaronasAnuaisAceitas.Add(8, 0);
			this.CaronasAnuaisAceitas.Add(9, 0);
			this.CaronasAnuaisAceitas.Add(10, 0);
			this.CaronasAnuaisAceitas.Add(11, 0);
			this.CaronasAnuaisAceitas.Add(12, 0);
		}

		private void InicializeCaronasAnuaisOferecidas()
		{
			this.CaronasAnuaisOferecidas = new Dictionary<int, int>();
			this.CaronasAnuaisOferecidas.Add(1, 0);
			this.CaronasAnuaisOferecidas.Add(2, 0);
			this.CaronasAnuaisOferecidas.Add(3, 0);
			this.CaronasAnuaisOferecidas.Add(4, 0);
			this.CaronasAnuaisOferecidas.Add(5, 0);
			this.CaronasAnuaisOferecidas.Add(6, 0);
			this.CaronasAnuaisOferecidas.Add(7, 0);
			this.CaronasAnuaisOferecidas.Add(8, 0);
			this.CaronasAnuaisOferecidas.Add(9, 0);
			this.CaronasAnuaisOferecidas.Add(10, 0);
			this.CaronasAnuaisOferecidas.Add(11, 0);
			this.CaronasAnuaisOferecidas.Add(12, 0);
		}

		private void InicializeLucroAnualAceitas()
		{
			this.LucroAnualAceitas = new Dictionary<int, double>();
			this.LucroAnualAceitas.Add(1, 0);
			this.LucroAnualAceitas.Add(2, 0);
			this.LucroAnualAceitas.Add(3, 0);
			this.LucroAnualAceitas.Add(4, 0);
			this.LucroAnualAceitas.Add(5, 0);
			this.LucroAnualAceitas.Add(6, 0);
			this.LucroAnualAceitas.Add(7, 0);
			this.LucroAnualAceitas.Add(8, 0);
			this.LucroAnualAceitas.Add(9, 0);
			this.LucroAnualAceitas.Add(10, 0);
			this.LucroAnualAceitas.Add(11, 0);
			this.LucroAnualAceitas.Add(12, 0);
		}

		private void InicializeLucroAnualOferecidas()
		{
			this.LucroAnualOferecidas = new Dictionary<int, double>();
			this.LucroAnualOferecidas.Add(1, 0);
			this.LucroAnualOferecidas.Add(2, 0);
			this.LucroAnualOferecidas.Add(3, 0);
			this.LucroAnualOferecidas.Add(4, 0);
			this.LucroAnualOferecidas.Add(5, 0);
			this.LucroAnualOferecidas.Add(6, 0);
			this.LucroAnualOferecidas.Add(7, 0);
			this.LucroAnualOferecidas.Add(8, 0);
			this.LucroAnualOferecidas.Add(9, 0);
			this.LucroAnualOferecidas.Add(10, 0);
			this.LucroAnualOferecidas.Add(11, 0);
			this.LucroAnualOferecidas.Add(12, 0);
		}
	}
}