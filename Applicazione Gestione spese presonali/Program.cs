using System;
using System.Diagnostics;
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
            Console.WriteLine("6. Ordina per data");
            Console.WriteLine("7. Bilancio mensile tra due date");
            Console.WriteLine("0. Esci");
            Console.Write("Scelta: ");
            scelta = Console.ReadLine();

            switch (scelta)
            {
                case "1":
                    bool cn = false;
                    int n;
                    do
                    {
                        Console.Write("Inserisci il numero di spese da inserire: ");
                        cn = int.TryParse(Console.ReadLine(), out n);
                        if (n <= 0)
                        {
                            Console.WriteLine("Il numero di spese deve essere maggiore di zero. Riprova.");
                        }
                        else if (cn == false)
                        {
                            Console.WriteLine($"Puoi inserire al massimo {100 - N} spese. Riprova.");
                            cn = false;
                        }
                        else
                        {
                            cn = true;
                        }

                    } while (cn == false || n <= 0);









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

                        Console.Write("Inserisci l'indice della spesa da eliminare: ");
                        // Controllo se l'input è un numero valido
                        validIndex = int.TryParse(Console.ReadLine(), out index);


                        if (index <= 0 || index > N || validIndex == false)
                        {
                            Console.WriteLine("Indice non valido. Riprova.");
                            validIndex = false;
                        }

                        else
                        {
                            validIndex = true;
                        }



                    } while (validIndex == false);


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

                    bool cdi;
                    bool cdf;
                    DateTime dataInizio;
                    DateTime dataFine;
                    do
                  {     
                    Console.WriteLine("Inserici la data di inizio: ");
                    string DataInizio = Console.ReadLine();
                    Console.WriteLine("Inserici la data di fine: ");
                    string DataFine = Console.ReadLine();
                        cdi = DateTime.TryParse(DataInizio, out dataInizio);
                        cdf = DateTime.TryParse(DataFine, out dataFine);
                        if (dataInizio > dataFine)
                        {
                            Console.WriteLine("La data di inizio non può essere successiva alla data di fine.");
                            
                        }

                        if (cdi==false||cdf==false)
                        {
                            Console.WriteLine("Formato date non valido.");
                          
                       
                        }

                   
                    } while (cdi == false || cdf == false|| dataInizio > dataFine);
                    
                    BilancioMensile(transazioni, N, dataInizio, dataFine);
                    break;

                case "0":
                    break;
                default:
                    Console.WriteLine("Scelta non valida.");
                    break;
            }

            if (modifiche)
            {
                bool cr;
                char risp;
                do
                {
                    Console.WriteLine("Vuoi salvare le modifiche? (s/n)");
                    cr = char.TryParse(Console.ReadLine(), out risp);
                    if (risp != 'S' || risp != 's' || risp != 'N' || risp != 'n'||cr == false)
                    {
                        Console.WriteLine("Input non valido");
                    }



                } while (risp != 'S' || risp != 's' || risp != 'N' || risp != 'n'|| cr == false);
                bool cc;
                char conf;
                do
                {
                    Console.WriteLine("Vuoi confermare ? (s/n)");
                    cc = char.TryParse(Console.ReadLine(), out conf);
                    if (conf != 'S' || conf != 's' || conf != 'N' || conf != 'n' || cc == false)
                    {
                        Console.WriteLine("Input non valido");
                    }



                } while (conf != 'S' || conf != 's' || conf != 'N' || conf != 'n' || cc == false);

                if (conf == 's' || conf == 'S')
                {
                    salvaSpese(transazioni, N, fileDati);
                    modifiche = false;
                    Console.WriteLine("Dati salvati con successo");
                }
            }

        } while (scelta != "0");
    }

    static void inserisciSpese(Transazione[] spese, ref int N, int n)
    {
        bool cs = false;
        for (int i = 0; i < n; i++)
        {
            Console.Write("Descrizione spesa: ");
            spese[N].descrizione = Console.ReadLine();

            Console.Write("Importo (€)(scrivi - se si tratta di una spesa): ");
            do
            {
                cs = double.TryParse(Console.ReadLine(), out spese[N].importo);
                if (cs == false)
                {
                    Console.WriteLine("Errore: Importo non valido. Riprova.");
                }
                
                else
                {
                    cs = true;
                }


            } while (cs==false);

            spese[N].data = DateTime.Now.Date;
            
            if(N > 100)
            {
                N = N + n;
            }
            
        }
    }

    static void visualizzaSpese(Transazione[] spese, int N)
    {

        for (int i = 0; i < N; i++)
        {
            int index = i + 1;
            Console.WriteLine($"{index}. {spese[i].descrizione} -  {spese[i].importo}Euro - {spese[i].data:dd/MM/yyyy}");
        }
    }

    static void modificaSpesa(Transazione[] spese, int N)
    {
        int ind;
        bool validIndex = false;
        do
        {
            Console.Write("Inserisci l'indice della spesa da modificare: ");
            
            validIndex = int.TryParse(Console.ReadLine(), out ind);

            if (ind <= 0 || ind > N || !validIndex)
            {
                Console.WriteLine("Indice non valido. Riprova.");
                validIndex = false;
            }
            else
            {
                validIndex = true;
            }

        } while (!validIndex);

        for (int i = 0; i < N; i++)
        {
            if (i == (ind - 1))
            {

                string scelta;
                Console.WriteLine($"Modifica spesa {ind}: {spese[i].descrizione} - {spese[i].importo} Euro - {spese[i].data:dd/MM/yyyy}");
                Console.WriteLine("Cosa vuoi modificare?");
                Console.WriteLine("1. Descrizione");
                Console.WriteLine("2. Importo");
                Console.WriteLine("3. Data");
                scelta = Console.ReadLine();
                switch (scelta)
                {
                    case "1":
                        Console.Write("Nuova descrizione: ");
                        spese[i].descrizione = Console.ReadLine();
                        break;
                    case "2":
                        Console.Write("Nuovo importo (€): ");
                        bool cs = false;
                        do
                        {
                            cs = double.TryParse(Console.ReadLine(), out spese[i].importo);
                            if (cs == false)
                            {
                                Console.WriteLine("Errore: Importo non valido. Riprova.");
                            }
                            else if (spese[i].importo < 0)
                            {
                                Console.WriteLine("L'importo non può essere negativo Riprova.");
                                cs = false;
                            }
                            else
                            {
                                cs = true;
                            }

                        } while (spese[i].importo < 0);
                        break;

                    case "3":
                        Console.Write("Nuova data (dd/MM/yyyy): ");
                        DateTime nuovaData;
                        while (!DateTime.TryParse(Console.ReadLine(), out nuovaData))
                        {
                            Console.WriteLine("Data non valida. Riprova.");
                        }
                        spese[i].data = nuovaData.Date;
                        break;

                    default:
                        Console.WriteLine("Scelta non valida.");
                        return;
                }
                Console.WriteLine("Spesa modificata.");
                return;
            }
        }

        Console.WriteLine("Spesa non trovata.");
    }

    static void eliminaSpesa(Transazione[] spese, ref int N, int ind)
    {
        for (int i = 0; i < N; i++)
        {
            if (i == (ind - 1))
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
                sw.WriteLine($"{spese[i].descrizione};{spese[i].importo};{spese[i].data:dd/MM/yyyy}");
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
        Console.WriteLine("Spese ordinate per data.");
    }


    private static void BilancioMensile(Transazione[] spese, int N, DateTime DI, DateTime  DF)
    {
       


        double sommaSpese = 0;
        double sommaEntrate = 0;

        for (int i = 0; i < N; i++)
        {
            if (spese[i].data >= DI && spese[i].data <= DF)
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


