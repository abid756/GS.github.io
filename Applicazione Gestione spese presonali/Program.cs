using System;
using System.IO;


internal class Program
{
    public struct Transazione
    {
        public string descrizione;
        public double importo;
        public DateTime data;
    }


    static void Main(string[] args)
    {
        Transazione[] transazioni = new Transazione[100];
        int N = 0;
        string fileDati = "spesa.txt";

        string scelta;
        bool modifiche = false;

        N = caricaTransazioni(transazioni, fileDati);

        do
        {
            Console.WriteLine("\n--- GESTIONE SPESE ---");
            Console.WriteLine("1. Inserisci spese");
            Console.WriteLine("2. Visualizza spese");
            Console.WriteLine("3. Modifica spesa");
            Console.WriteLine("4. Elimina spesa");
            Console.WriteLine("5. Salva spese");
            Console.WriteLine("6. Ordina per descrizione");
            Console.WriteLine("7. Bilancio mensile tra due date");
            Console.WriteLine("0. Esci");
            Console.Write("Scelta: ");
            scelta = Console.ReadLine();

            switch (scelta)
            {
                case "1":
                    Console.Write("Quante spese vuoi inserire? ");
                    int n = Convert.ToInt32(Console.ReadLine());
                    inserisciSpese(transazioni, ref N, n);
                    modifiche = true;
                    break;
                case "2":
                    visualizzaSpese(transazioni, N);
                    break;
                case "3":
                    modificaSpesa(transazioni, N);
                    modifiche = true;
                    break;
                case "4":
                    int index;
                    visualizzaSpese(transazioni, N);

                    bool validIndex = false;
                    do
                    {


                     validIndex= int.TryParse(Console.ReadLine(), out index);

                        
                        if (index <= 0 || index > N||validIndex== false)
                        {
                            Console.WriteLine("Indice non valido. Riprova.");
                            validIndex = false;
                        }
                      
                        else
                        {
                            validIndex = true;
                        }



                    } while (validIndex ==false);
                    

                    eliminaSpesa(transazioni, ref N, index);
                    modifiche = true;
                    break;
                case "5":
                    salvaSpese(transazioni, N, fileDati);
                    modifiche = false;
                    break;
                case "6":
                    ordinaSpese(transazioni, N);
                    break;

                case "7":
                    Console.WriteLine("Inserici la data di inizio: ");
                    string DataInizio = Console.ReadLine();
                    Console.WriteLine("Inserici la data di fine: ");
                    string DataFine = Console.ReadLine();


                    BilancioMensile(transazioni, N, DataInizio, DataFine);
                    break;

                case "0":
                    break;
                default:
                    Console.WriteLine("Scelta non valida.");
                    break;
            }

            if (modifiche)
            {
                Console.WriteLine("Vuoi salvare le modifiche? (s/n)");
                char risp = Convert.ToChar(Console.ReadLine());
                if (risp == 's' || risp == 'S')
                {
                    salvaSpese(transazioni, N, fileDati);
                    modifiche = false;
                }
            }

        } while (scelta != "0");
    }

    static void inserisciSpese(Transazione[] spese, ref int N, int n)
    {
        for (int i = 0; i < n; i++)
        {
            Console.Write("Descrizione spesa: ");
            spese[N].descrizione = Console.ReadLine();

            Console.Write("Importo (€)(scrivi - se si tratta di una spesa): ");
            do
            {
                try
                {
                    spese[N].importo = Convert.ToDouble(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Errore: inserire un numero valido.");
                    spese[N].importo = -1;
                }
            } while (spese[N].importo < 0);

            spese[N].data = DateTime.Now.Date;
            N++;
        }
    }

    static void visualizzaSpese(Transazione[] spese, int N)
    {
          
        for (int i = 0; i < N; i++)
        {
           int index = i+1;
            Console.WriteLine($"{index}. {spese[i].descrizione} -  {spese[i].importo}Euro - {spese[i].data:dd/MM/yyyy}");
        }
    }

    static void modificaSpesa(Transazione[] spese, int N)
    {
        Console.Write("Inserisci la descrizione della spesa da modificare: ");
        string desc = Console.ReadLine();

        for (int i = 0; i < N; i++)
        {
            if (spese[i].descrizione.Equals(desc, StringComparison.OrdinalIgnoreCase))
            {
                Console.Write("Nuova descrizione: ");
                spese[i].descrizione = Console.ReadLine();
                try
                {
                    Console.Write("Nuovo importo (€): ");
                    spese[i].importo = Convert.ToDouble(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("Errore:Importo non valido.");
                }
                Console.WriteLine("Spesa modificata con successo.");
                return;
            }
        }

        Console.WriteLine("Spesa non trovata.");
    }

    static void eliminaSpesa(Transazione[] spese, ref int N,int ind)
    {
        for (int i = 0; i < N; i++)
        {
            if (i == (ind -1))
            {
                for (int j = i; j < N - 1; j++)
                {
                    spese[j] = spese[j + 1];
                }
                N--;
                Console.WriteLine("Spesa eliminata.");
                return;
            }
        }
        Console.WriteLine("Spesa non trovata.");
    }

    static int caricaTransazioni(Transazione[] spese, string nomeFile)
    {
        int count = 0;
        try
        {
            using (StreamReader sr = new StreamReader(nomeFile))
            {
                string riga;
                while ((riga = sr.ReadLine()) != null)
                {
                    string[] dati = riga.Split(";");
                    spese[count].descrizione = dati[0];
                    spese[count].importo = Convert.ToDouble(dati[1]);
                    spese[count].data = DateTime.Parse(dati[2]);
                    count++;
                }
            }
        }
        catch
        {
            Console.WriteLine("Nessun file di spese trovato. Sarà creato al salvataggio.");
        }
        return count;
    }

    static void salvaSpese(Transazione[] spese, int N, string nomeFile)
    {
        using (StreamWriter sw = new StreamWriter(nomeFile))
        {
            for (int i = 0; i < N; i++)
            {
                sw.WriteLine($"{spese[i].descrizione};{spese[i].importo};{spese[i].data}");
            }
        }
        Console.WriteLine("Spese salvate correttamente.");
    }

    static void ordinaSpese(Transazione[] spese, int N)
    {
        for (int i = 0; i < N - 1; i++)
        {
            for (int j = 0; j < N - i - 1; j++)
            {
                if (spese[j].data > spese[j + 1].data)
                {
                    var temp = spese[j];
                    spese[j] = spese[j + 1];
                    spese[j + 1] = temp;
                }
            }
        }
        Console.WriteLine("Spese ordinate per descrizione.");
    }

    
    private static void BilancioMensile(Transazione[] spese, int N, string DI, string DF)
    {
        DateTime dataInizio = DateTime.Parse(DI);
        DateTime dataFine = DateTime.Parse(DF);

        double sommaSpese = 0;
        double sommaEntrate = 0;

        for (int i = 0; i < N; i++)
        {
            if (spese[i].data >= dataInizio && spese[i].data <= dataFine)
            {
                if (spese[i].importo < 0)
                {
                    sommaSpese += spese[i].importo;
                }
                else
                {
                    sommaEntrate += spese[i].importo;
                }
            }
        }

        double bilancio = sommaEntrate + sommaSpese;
        Console.WriteLine($"Bilancio tra entrate e spese: {bilancio:F2} Euro");
    }







}









