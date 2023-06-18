using System;

using System.Collections.Generic;



namespace DomuzBankProje

{

    // Para türleri

    public enum ParaTipi

    {

        Bozuk,

        Banknote

    }



    // Para sınıfı

    public abstract class Para

    {

        public string Ad { get; set; }

        public decimal Deger { get; set; }

        public ParaTipi ParaTipi { get; set; }

        public decimal Hacim { get; set; }

    }



    // Kağıt para sınıfı

    public class Banknote : Para

    {

        public Banknote()

        {

            Hacim = 0.25m * Deger;

        }



        // Para katlandığında kapladığı yeri günceller

        public void Fold()

        {

            Hacim *= 4;

        }

    }



    // Bozuk para sınıfı

    public class Bozuk : Para

    {

        public Bozuk()

        {

            const decimal pi = 3.14m;

            Hacim = pi * (Deger / 2) * (Deger / 2) * Deger;

        }

    }



    // Kumbara sınıfı

    public class DomuzBank

    {

        private List<Para> ParaList;

        public decimal MaxHacim { get; }

        public decimal MevcutHacim { get; private set; }



        public DomuzBank(decimal maxHacim)

        {

            MaxHacim = maxHacim;

            MevcutHacim = 0;

            ParaList = new List<Para>();

        }



        public void ParaEkle(Para Para)

        {

            decimal guncelleHacim = MevcutHacim + Para.Hacim;



            if (guncelleHacim > MaxHacim)

            {

                throw new Exception("Kumbara hacmi dolu, para atılamaz!");

            }



            // Para türüne göre katlama işlemi yapılır

            if (Para is Banknote)

            {

                Banknote banknote = (Banknote)Para;

                banknote.Fold();

            }



            MevcutHacim = guncelleHacim;

            ParaList.Add(Para);

        }



        public List<Para> Kır()

        {

            List<Para> returnedPara = new List<Para>(ParaList);

            ParaList.Clear();

            MevcutHacim = 0;

            return returnedPara;

        }



        public void Salla()

        {

            foreach (var Para in ParaList)

            {

                // Atılan paranın hacminin %25-%75 oranında rastgele bir fazladan hacim oluşturulur

                decimal yeniHacim = Para.Hacim * new Random().Next(25, 75) / 100;

                Para.Hacim -= yeniHacim;

            }

        }



        public decimal ToplamMiktar()

        {

            decimal miktar = 0;

            foreach (var Para in ParaList)

            {

                miktar += Para.Deger;

            }

            return miktar;

        }

    }



    class Program

    {

        static void Main(string[] args)

        {

            try

            {

                DomuzBank DomuzBank = new DomuzBank(1000); // Maksimum hacmi 1000 birim olarak belirle



                while (true)

                {

                    Console.WriteLine("Yapmak istediğiniz işlemi seçin:");

                    Console.WriteLine("1 - Para ekle");

                    Console.WriteLine("2 - Para say");

                    Console.WriteLine("3 - Kumbarayı kır ve çıkan paraları göster");

                    Console.WriteLine("0 - Çıkış");



                    int secim = Convert.ToInt32(Console.ReadLine());



                    switch (secim)

                    {

                        case 1:

                            Console.WriteLine("Eklenecek para miktarını girin:");

                            decimal amount = Convert.ToDecimal(Console.ReadLine());



                            Console.WriteLine("Para türünü seçin:");

                            Console.WriteLine("1 - Bozuk para");

                            Console.WriteLine("2 - Kağıt para");

                            int ParaTipisecim = Convert.ToInt32(Console.ReadLine());



                            if (ParaTipisecim == 1)

                            {

                                Bozuk Bozuk = new Bozuk();

                                Bozuk.Deger = amount;

                                DomuzBank.ParaEkle(Bozuk);

                            }

                            else if (ParaTipisecim == 2)

                            {

                                Banknote banknote = new Banknote();

                                banknote.Deger = amount;

                                DomuzBank.ParaEkle(banknote);

                            }

                            else

                            {

                                Console.WriteLine("Geçersiz para türü seçimi!");

                                continue;

                            }



                            if (DomuzBank.MevcutHacim >= DomuzBank.MaxHacim)

                            {

                                DomuzBank.Salla();

                            }



                            Console.WriteLine("Para eklendi.");

                            break;



                        case 2: //bu case kod kontrol için eklendi

                            decimal miktar = DomuzBank.ToplamMiktar();

                            Console.WriteLine($"Kumbaradaki toplam para miktarı: {miktar} birim");

                            break;



                        case 3:

                            List<Para> returnedPara = DomuzBank.Kır();

                            Console.WriteLine("Kumbaradan çıkan paralar:");

                            foreach (var Para in returnedPara)

                            {

                                Console.WriteLine($"{Para.Ad} - {Para.Deger} -{Para.ParaTipi} TL");

                            }

                            break;



                        case 0:

                            return;



                        default:

                            Console.WriteLine("Geçersiz seçim!");

                            break;

                    }

                }

            }

            catch (Exception ex)

            {

                Console.WriteLine($"Hata: {ex.Message}");

            }

        }

    }

}