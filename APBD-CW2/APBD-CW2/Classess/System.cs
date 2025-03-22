namespace APBD_CW2.Classess;

public class System
{
    public ICollection<ContainerShip> Ships { get; set; } = new List<ContainerShip>();
    public ICollection<Container> Containers { get; set; } = new List<Container>();

    public System() {}

    private void CreateContainer()
    {
        Console.WriteLine("Typ kontenera: A - Gaz, B - Płyn, C - Chłodniczy");
        string type = Console.ReadLine().ToUpper();

        Console.WriteLine("Podaj wagę:");
        int weight = int.Parse(Console.ReadLine());
        Console.WriteLine("Podaj wysokość:");
        int height = int.Parse(Console.ReadLine());
        Console.WriteLine("Podaj wagę kontenera:");
        int containerWeight = int.Parse(Console.ReadLine());
        Console.WriteLine("Podaj głębokość:");
        int depth = int.Parse(Console.ReadLine());
        
        string serial;
        while (true)
        {
            Console.WriteLine("Podaj numer seryjny (KON-X-Y):");
            serial = Console.ReadLine();

            bool exists = Containers.Any(c => c.SerialNumber == serial)
                          || Ships.SelectMany(s => s.Containers).Any(c => c.SerialNumber == serial);

            if (exists)
            {
                Console.WriteLine("Błąd: kontener o tym numerze seryjnym już istnieje. Spróbuj ponownie.");
            }
            else break;
        }
        
        Console.WriteLine("Podaj maksymalną wagę:");
        int maxWeight = int.Parse(Console.ReadLine());

        switch (type)
        {
            case "A":
                Console.WriteLine("Podaj ciśnienie:");
                int pressure = int.Parse(Console.ReadLine());
                Containers.Add(new ContainerGas(weight, height, containerWeight, depth, serial, maxWeight, pressure));
                break;
            case "B":
                Console.WriteLine("Czy niebezpieczny (true/false):");
                bool dangerous = bool.Parse(Console.ReadLine());
                Containers.Add(new ContainerLiquid(weight, height, containerWeight, depth, serial, maxWeight, dangerous));
                break;
            case "C":
                Console.WriteLine("Podaj produkt (np. Bananas):");
                var product = Enum.Parse<Product.Product>(Console.ReadLine());
                Console.WriteLine("Podaj temperaturę:");
                int temp = int.Parse(Console.ReadLine());
                Containers.Add(new ContainerReefer(weight, height, containerWeight, depth, serial, maxWeight, product, temp));
                break;
            default:
                Console.WriteLine("Nieznany typ kontenera.");
                break;
        }
    }
    
    private void AddShip()
    {
        Console.WriteLine("Podaj maksymalną prędkość:");
        int speed = int.Parse(Console.ReadLine());
        Console.WriteLine("Podaj maksymalną liczbę kontenerów:");
        int limit = int.Parse(Console.ReadLine());
        Console.WriteLine("Podaj maksymalną wagę:");
        int weight = int.Parse(Console.ReadLine());
        Ships.Add(new ContainerShip(new List<Container>(), speed, limit, weight));
    }
    
    private void RemoveShip()
    {
        Console.WriteLine("Podaj numer statku do usunięcia:");
        int index = int.Parse(Console.ReadLine()) - 1;
        if (index >= 0 && index < Ships.Count)
            Ships.Remove(Ships.ElementAt(index));
        else
            Console.WriteLine("Niepoprawny numer statku.");
    }
    
    private void LoadContainer()
    {
        Console.WriteLine("Podaj numer kontenera:");
        string serial = Console.ReadLine();
        var container = Containers.FirstOrDefault(c => c.SerialNumber == serial);
        if (container == null) return;

        Console.WriteLine("Podaj numer statku:");
        int shipIndex = int.Parse(Console.ReadLine()) - 1;
        if (shipIndex < 0 || shipIndex >= Ships.Count) return;

        var ship = Ships.ElementAt(shipIndex);
        if (ship.Containers.Count >= ship.Limit || ship.Containers.Sum(c => c.Weight) + container.Weight > ship.MaxWeight)
        {
            Console.WriteLine("Statek jest przeciążony.");
            return;
        }

        ship.Containers.Add(container);
        Containers.Remove(container);
    }
    
    private void LoadAllContainers()
    {
        Console.WriteLine("Podaj numer statku:");
        int index = int.Parse(Console.ReadLine()) - 1;
        if (index < 0 || index >= Ships.Count) return;

        var ship = Ships.ElementAt(index);
        foreach (var container in Containers.ToList())
        {
            if (ship.Containers.Count >= ship.Limit || ship.Containers.Sum(c => c.Weight) + container.Weight > ship.MaxWeight) continue;
            ship.Containers.Add(container);
            Containers.Remove(container);
        }
    }
    
    private void RemoveContainer()
    {
        Console.WriteLine("Podaj numer statku:");
        int index = int.Parse(Console.ReadLine()) - 1;
        if (index < 0 || index >= Ships.Count) return;

        var ship = Ships.ElementAt(index);
        Console.WriteLine("Podaj numer seryjny kontenera:");
        string serial = Console.ReadLine();

        var container = ship.Containers.FirstOrDefault(c => c.SerialNumber == serial);
        if (container == null) return;

        ship.Containers.Remove(container);
        Containers.Add(container);
    }
    
    private void UnloadContainer()
    {
        Console.WriteLine("Podaj numer seryjny kontenera:");
        string serial = Console.ReadLine();
        var container = Containers.FirstOrDefault(c => c.SerialNumber == serial)
                        ?? Ships.SelectMany(s => s.Containers).FirstOrDefault(c => c.SerialNumber == serial);
        if (container != null)
        {
            container.EmptyContainer();
            Console.WriteLine("Rozładowano kontener.");
        }
    }
    
    private void ReplaceContainer()
    {
        Console.WriteLine("Podaj numer statku:");
        int index = int.Parse(Console.ReadLine()) - 1;
        if (index < 0 || index >= Ships.Count) return;

        var ship = Ships.ElementAt(index);
        Console.WriteLine("Numer kontenera do zastąpienia:");
        string oldSerial = Console.ReadLine();
        var oldContainer = ship.Containers.FirstOrDefault(c => c.SerialNumber == oldSerial);
        if (oldContainer == null) return;

        Console.WriteLine("Numer nowego kontenera:");
        string newSerial = Console.ReadLine();
        var newContainer = Containers.FirstOrDefault(c => c.SerialNumber == newSerial);
        if (newContainer == null) return;

        ship.Containers.Remove(oldContainer);
        ship.Containers.Add(newContainer);
        Containers.Remove(newContainer);
        Containers.Add(oldContainer);
    }
    
    private void MoveContainer()
    {
        Console.WriteLine("Numer statku źródłowego:");
        int from = int.Parse(Console.ReadLine()) - 1;
        Console.WriteLine("Numer statku docelowego:");
        int to = int.Parse(Console.ReadLine()) - 1;
        if (from < 0 || from >= Ships.Count || to < 0 || to >= Ships.Count) return;

        var source = Ships.ElementAt(from);
        var target = Ships.ElementAt(to);

        Console.WriteLine("Numer kontenera:");
        string serial = Console.ReadLine();
        var container = source.Containers.FirstOrDefault(c => c.SerialNumber == serial);
        if (container == null) return;

        if (target.Containers.Count >= target.Limit || target.Containers.Sum(c => c.Weight) + container.Weight > target.MaxWeight)
        {
            Console.WriteLine("Nie można przenieść - statek przeciążony.");
            return;
        }

        source.Containers.Remove(container);
        target.Containers.Add(container);
    }
    
    private void ShowContainerInfo()
    {
        Console.WriteLine("Numer seryjny kontenera:");
        string serial = Console.ReadLine();
        var container = Containers.FirstOrDefault(c => c.SerialNumber == serial)
                        ?? Ships.SelectMany(s => s.Containers).FirstOrDefault(c => c.SerialNumber == serial);
        if (container != null)
        {
            Console.WriteLine($"Serial: {container.SerialNumber}, Typ: {container.GetType().Name}, Waga: {container.Weight}, Max Weight: {container.MaxWeight}");
        }
    }
    
    private void ShowShipInfo()
    {
        Console.WriteLine("Podaj numer statku:");
        int index = int.Parse(Console.ReadLine()) - 1;
        if (index < 0 || index >= Ships.Count) return;

        var ship = Ships.ElementAt(index);
        Console.WriteLine($"Statek: Speed={ship.MaxSpeed}, MaxWeight={ship.MaxWeight}, Limit={ship.Limit}, count={ship.Containers.Count}, weight={ship.Containers.Sum(c => c.Weight)}");
        foreach (var c in ship.Containers)
        {
            Console.WriteLine($" - {c.SerialNumber}, {c.GetType().Name}, Waga: {c.Weight}");
        }
    }
    
    public void Start()
{
    while (true)
    {
        Console.Clear();

        Console.WriteLine("Lista kontenerowców:");
        if (!Ships.Any())
            Console.WriteLine("Brak");
        else
        {
            int i = 1;
            foreach (var ship in Ships)
            {
                Console.WriteLine($"{i++}. Speed={ship.MaxSpeed}, MaxWeight={ship.MaxWeight}, Limit={ship.Limit}");
            }
        }

        Console.WriteLine("\nLista kontenerów:");
        if (!Containers.Any())
            Console.WriteLine("Brak");
        else
        {
            foreach (var container in Containers)
            {
                Console.WriteLine($"{container.SerialNumber} ({container.GetType().Name})");
            }
        }

        Console.WriteLine("\nMożliwe akcje:");
        Console.WriteLine("1. Dodaj kontenerowiec");
        Console.WriteLine("2. Usuń kontenerowiec");
        Console.WriteLine("3. Dodaj kontener");
        Console.WriteLine("4. Załaduj kontener na statek");
        Console.WriteLine("5. Załaduj listę kontenerów na statek");
        Console.WriteLine("6. Usuń kontener ze statku");
        Console.WriteLine("7. Rozładuj kontener");
        Console.WriteLine("8. Zastąp kontener na statku");
        Console.WriteLine("9. Przenieś kontener między statkami");
        Console.WriteLine("10. Info o kontenerze");
        Console.WriteLine("11. Info o statku");
        Console.WriteLine("0. Wyjdź");
        Console.Write("\nWybierz opcję: ");

        string? choice = Console.ReadLine();
        Console.WriteLine();

        try
        {
            switch (choice)
            {
                case "1": AddShip(); break;
                case "2": RemoveShip(); break;
                case "3": CreateContainer(); break;
                case "4": LoadContainer(); break;
                case "5": LoadAllContainers(); break;
                case "6": RemoveContainer(); break;
                case "7": UnloadContainer(); break;
                case "8": ReplaceContainer(); break;
                case "9": MoveContainer(); break;
                case "10": ShowContainerInfo(); break;
                case "11": ShowShipInfo(); break;
                case "0": return;
                default: Console.WriteLine("Niepoprawna opcja."); break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        

        Console.WriteLine("\nNaciśnij dowolny klawisz, aby kontynuować...");
        Console.ReadKey();
    }
}
}