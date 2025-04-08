using System.Text.Json;
using System.Text.Json.Serialization;

TaskManager taskManager = new TaskManager();

while (true)
{
    taskManager.LoadTasks();
    Menu();
    string input = Console.ReadLine();

    switch (input)
    {
        case "1":
            Console.Clear();
            ShowTaskList();
            break;
        case "2":
            Console.Clear();
            Task newTask = new Task();
            Console.Write("Un nom > ");
            string name = Console.ReadLine();
            Console.Write("Une description > ");
            string description = Console.ReadLine();
            
            newTask.SetName(name);
            newTask.SetDescription(description);
            taskManager.AddTaskToList(newTask);
            taskManager.SaveTasks();
            
            Console.WriteLine("Tâche créer avec succès");
            Console.WriteLine(name + " - " + description);

            break;
        case "3":
            Console.Clear();
            ShowTaskList();
            Console.WriteLine("+|+ Indiquez le numero de la tâche à modifier +|+");
            int choiceUser = int.Parse(Console.ReadLine());
            Console.WriteLine("Choix de la tâche : " + taskManager.TasksList()[choiceUser - 1].GetName());
            Console.Write("Nouveau nom > ");
            string newName = Console.ReadLine();
            Console.Write("Nouvelle description > ");
            string desc = Console.ReadLine();
            Task task = taskManager.TasksList()[choiceUser - 1];
            if (newName != null)
            {
                Console.WriteLine("NOM");
                taskManager.UpdateTask(task, name: newName);
            }
            
            else if (desc != null)
            {
                Console.WriteLine("DES");
                taskManager.UpdateTask(task, description: desc);
            }
            else
            {
                Console.WriteLine("NOM ET DESC");
                taskManager.UpdateTask(task, name: newName, description: desc);
            }
            taskManager.SaveTasks();
            Console.WriteLine("La tâche est desormais modifier :");
            Console.WriteLine(taskManager.TasksList()[choiceUser - 1].GetName() + " : " + taskManager.TasksList()[choiceUser - 1].GetDescription());
            break;
        case "4":
            Console.Clear();
            ShowTaskList();
            Console.WriteLine("+|+ Indiquez le numero de la tâche à modifier +|+");
            int choiceUserValidatedTask = int.Parse(Console.ReadLine());
            taskManager.ValidateTask(taskManager.TasksList()[choiceUserValidatedTask - 1]);
            taskManager.SaveTasks();
            break;
        case "5":
            Environment.Exit(0);
            break;
        default:
            Menu();
            break;
    }
}

return;

void Menu()
{
    Console.WriteLine("===== TodoList =====");
    Console.WriteLine("1. Liste des tâches");
    Console.WriteLine("2. Créer une tâches");
    Console.WriteLine("3. Gérer des tâches");
    Console.WriteLine("4. Valider des tâches");
    Console.WriteLine("5. Quitter");
    Console.Write(" > ");
}

void ShowTaskList()
{
    for (int i = 0; i < taskManager.TasksList().Count; i++)
    {
        Console.WriteLine((i+1) + ". [" + taskManager.TasksList()[i].GetState() + "]" + taskManager.TasksList()[i].GetName() + " : " + taskManager.TasksList()[i].GetDescription());
    }
}

string TasksList()
{
    return taskManager.TasksList().ToString();
}

public enum STATE
{
    Open, //0
    Progress, //1
    Done //2
}

public class Task
{
    public int _id { get; set; }
    public string _category { get; set; }
    public string _name { get; set; }
    public string _description { get; set; }
    public STATE _state { get; set; }

    public Task()
    {
        _state = STATE.Open;
        _category = "Categorie par défaut";
        _name = "Tâche par défaut";
        _description = "Description de la tâche";
    }

    public string GetName()
    {
        return _name;
    }

    public string GetCategory()
    {
        return _category;
    }

    public string GetDescription()
    {
        return _description;
    }

    public STATE GetState()
    {
        return _state;
    }

    public void SetName(string name)
    {
        _name = name;
    }

    public void SetCategory(string category)
    {
        _category = category;
    }

    public void SetDescription(string description)
    {
        _description = description;
    }

    public void SetState(STATE state)
    {
        _state = state;
    }
}

public class TaskManager
{
    public List<Task> Tasks { get; set; } = new();

    public List<Task> TasksList()
    {
        return Tasks;
    }

    public void AddTaskToList(Task task)
    {
        Tasks.Add(task);
    }

    public void UpdateTask(Task task, string name="", string description="")
    {
        task.SetName(name);
        task.SetDescription(description);
    }

    public void ValidateTask(Task task)
    {
        task.SetState(STATE.Done);
    }

    public void SortTasks()
    {
        Tasks.OrderBy(task => task.GetState());
    }

    public void SaveTasks()
    {
        string json = JsonSerializer.Serialize(Tasks);
        File.WriteAllText(@"C:\Users\s_13459_gamedev\Documents\tasks.json", json);
    }

    public void LoadTasks()
    {
        string json = File.ReadAllText(@"C:\Users\s_13459_gamedev\Documents\tasks.json");
        Tasks = JsonSerializer.Deserialize<List<Task>>(json);
    }
}