using System;

abstract class Worker
{
    public string Name { get; private set; }
    public string Position { get; protected set; }
    public string WorkDay { get; private set; }
    public Worker(string name)
    {
        Name = name;
        WorkDay = string.Empty;
    }
    public void Call()
    {
        WorkDay += "Call ";
        Console.WriteLine($"{Name} is making a call");
    }
    public void WriteCode()
    {
        WorkDay += "WriteCode ";
        Console.WriteLine($"{Name} is writing code");
    }
    public void Relax()
    {
        WorkDay += "Relax ";
        Console.WriteLine($"{Name} is relaxing");
    }
    public abstract void FillWorkDay();
}

class Developer : Worker
{
    public Developer(string name) : base(name)
    {
        Position = "Розробник";
    }
    public override void FillWorkDay()
    {
        WriteCode();
        Call();
        Relax();
        WriteCode();
    }
}

class Manager : Worker
{
    private Random random = new Random();
    public Manager(string name) : base(name)
    {
        Position = "Менеджер";
    }
    public override void FillWorkDay()
    {
        int callsBeforeRelax = random.Next(1, 11);
        for (int i = 0; i < callsBeforeRelax; i++)
        {
            Call();
        }
        Relax();

        int callsAfterRelax = random.Next(1, 6);
        for (int i = 0; i < callsAfterRelax; i++)
        {
            Call();
        }
    }
}

class Team
{
    public string TeamName { get; private set; }
    private List<Worker> workers;

    public Team(string teamName)
    {
        TeamName = teamName;
        workers = new List<Worker>();
    }

    public void AddWorker(Worker worker)
    {
        workers.Add(worker);
    }

    public int WorkersCount()
    {
        return workers.Count;
    }

    public Worker GetWorker(int index)
    {
        if (index < 0 || index >= workers.Count)
            return null;
        return workers[index];
    }

    public void ShowTeamInfo()
    {
        Console.WriteLine($"Team: {TeamName}");
        foreach (var worker in workers)
        {
            Console.WriteLine(worker.Name);
        }
    }

    public void ShowDetailedTeamInfo()
    {
        Console.WriteLine($"Team: {TeamName}");
        foreach (var worker in workers)
        {
            Console.WriteLine($"{worker.Name} - {worker.Position} - {worker.WorkDay}");
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        List<Team> teams = new List<Team>();

        while (true)
        {
            Console.WriteLine("1. Додати команду");
            Console.WriteLine("2. Додати співробітника до команди");
            Console.WriteLine("3. Подивитися інформацію про команду");
            Console.WriteLine("4. Подивитися інформацію про співробітника");
            Console.WriteLine("5. Вийти");
            Console.WriteLine("Оберіть дію:");

            int choice;
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Невірна дія");
                continue;
            }

            switch (choice)
            {
                case 1:
                    AddNewTeam(teams);
                    break;
                case 2:
                    AddWorkerToTeam(teams);
                    break;
                case 3:
                    ShowTeamsInfo(teams);
                    break;
                case 4:
                    ShowWorkersInfo(teams);
                    break;
                case 5:
                    return;
                default:
                    Console.WriteLine("Невірна дія");
                    break;
            }
        }
    }

    static void AddNewTeam(List<Team> teams)
    {
        Console.Write("Введіть назву команди:");
        string teamName = Console.ReadLine();
        Team team = new Team(teamName);
        teams.Add(team);
        Console.WriteLine($"Команду '{teamName}' додано");
    }

    static void AddWorkerToTeam(List<Team> teams)
    {
        if (teams.Count == 0)
        {
            Console.WriteLine("У вас немає команд");
            return;
        }

        Console.WriteLine("Доступні команди:");
        for (int i = 0; i < teams.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {teams[i].TeamName}");
        }

        Console.Write("Оберіть команду:");
        int teamIndex;
        if (!int.TryParse(Console.ReadLine(), out teamIndex) || teamIndex < 1 || teamIndex > teams.Count)
        {
            Console.WriteLine("Невірний номер команди");
            return;
        }

        Team selectedTeam = teams[teamIndex - 1];

        Console.Write("Введіть ім'я співробітника:");
        string workerName = Console.ReadLine();

        Console.WriteLine("1. Розробник");
        Console.WriteLine("2. Менеджер");
        Console.WriteLine("Оберіть посаду співробітника:");
        int role;
        if (!int.TryParse(Console.ReadLine(), out role))
        {
            Console.WriteLine("Невірна посада");
            return;
        }

        Worker worker = null;
        switch (role)
        {
            case 1:
                worker = new Developer(workerName);
                break;
            case 2:
                worker = new Manager(workerName);
                break;
            default:
                Console.WriteLine("Невірна роль");
                return;
        }

        selectedTeam.AddWorker(worker);
        worker.FillWorkDay();
        Console.WriteLine($"Співробітника '{workerName}' додано до команди '{selectedTeam.TeamName}'");
    }

    static void ShowTeamsInfo(List<Team> teams)
    {
        if (teams.Count == 0)
        {
            Console.WriteLine("У вас немає команд");
            return;
        }

        Console.WriteLine("Доступні команди:");
        for (int i = 0; i < teams.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {teams[i].TeamName}");
        }

        Console.WriteLine("Оберіть команду:");
        int teamIndex;
        if (!int.TryParse(Console.ReadLine(), out teamIndex) || teamIndex < 1 || teamIndex > teams.Count)
        {
            Console.WriteLine("Невірний номер команди");
            return;
        }

        Team selectedTeam = teams[teamIndex - 1];

        Console.WriteLine("1. Звичайна");
        Console.WriteLine("2. Детальна");
        Console.Write("Оберіть інформацію:");

        int infoChoice;
        if (!int.TryParse(Console.ReadLine(), out infoChoice))
        {
            Console.WriteLine("Невірний вибір. Показується звичайна інформація");
            infoChoice = 1;
        }

        switch (infoChoice)
        {
            case 1:
                selectedTeam.ShowTeamInfo();
                break;
            case 2:
                selectedTeam.ShowDetailedTeamInfo();
                break;
            default:
                Console.WriteLine("Невірний вибір. Показується звичайна інформація");
                selectedTeam.ShowTeamInfo();
                break;
        }
    }

    static void ShowWorkersInfo(List<Team> teams)
    {
        if (teams.Count == 0)
        {
            Console.WriteLine("У вас немає команд");
            return;
        }

        Console.WriteLine("Доступні команди:");
        for (int i = 0; i < teams.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {teams[i].TeamName}");
        }

        Console.Write("Оберіть команду:");
        int teamIndex;
        if (!int.TryParse(Console.ReadLine(), out teamIndex) || teamIndex < 1 || teamIndex > teams.Count)
        {
            Console.WriteLine("Невірний номер команди");
            return;
        }

        Team selectedTeam = teams[teamIndex - 1];

        if (selectedTeam.WorkersCount() == 0)
        {
            Console.WriteLine("У команді немає співробітників");
            return;
        }

        Console.WriteLine("Співробітники команди:");
        for (int i = 0; i < selectedTeam.WorkersCount(); i++)
        {
            Console.WriteLine($"{i + 1}. {selectedTeam.GetWorker(i).Name}");
        }

        Console.Write("Оберіть співробітника:");
        int workerIndex;
        if (!int.TryParse(Console.ReadLine(), out workerIndex) || workerIndex < 1 || workerIndex > selectedTeam.WorkersCount())
        {
            Console.WriteLine("Невірний номер співробітника");
            return;
        }

        Worker selectedWorker = selectedTeam.GetWorker(workerIndex - 1);
        Console.WriteLine($"Інформація про співробітника: {selectedWorker.Name} - {selectedWorker.Position} - {selectedWorker.WorkDay}");
    }
}