// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using System.Text;

namespace TaskManagementSystem
{
    //We create a public class that will contain all our task management propt
    public class TaskContainer
    {
        public string title;
        public string description;
        public DateTime deadline;
        public int priority;
        public string category;
        public bool isCompleted;
    }

    class Program
    {
        static List<TaskContainer> container = new List<TaskContainer>();
        static void Main(String[] args)
        {
            Banner();
            bool runInBg = true;//keeps the application running
            while (runInBg)
            {
                //while the application is runninng,diplay the menu and change the color of the ui to add more beautity to our ui
                Menu();

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                int selected = int.Parse(Console.ReadLine());
                Console.ResetColor();

                switch (selected)
                {
                    case 1:
                        SaveTask();
                        break;
                    case 2:
                        LoadTask();
                        break;
                    case 3:
                        AddTask();
                        break;
                    case 4:
                        ViewTask();
                        break;
                    case 5:
                        EditTask();
                        break;
                    case 6:
                        TaskCompleted();
                        break;
                    case 7:
                        FilterTask();
                        break;
                    case 8:
                        SortTask();
                        break;
                    case 9:
                        RemoveTask();
                        break;
                    case 10:
                        runInBg = false;
                        break;
                    default:
                        Console.WriteLine("Invalid Response!!!");
                        break;
                }
            }
        }
        static void Menu()
        {
            //here we list our task management details
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Select an Option");
            Console.WriteLine("#1 Save Task");
            Console.WriteLine("#2 Load Tasks");
            Console.WriteLine("#3 Add Task");
            Console.WriteLine("#4 View Task");
            Console.WriteLine("#5 Edit Task");
            Console.WriteLine("#6 is Task Completed Task");
            Console.WriteLine("#7 Filter Task");
            Console.WriteLine("#8 Sort Task");
            Console.WriteLine("#9 Delete Task");
            Console.WriteLine("#10 Exit App");
            Console.ResetColor();
        }
        static void Banner()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("         TASK MANAGEMENT SYSTEM      ");
            Console.ResetColor();
            Console.WriteLine("*************************************s");
        }
        //loading saved data
        static void LoadTask()
        {
            //check is a saved data already exits
            if(File.Exists("task.txt"))
            {
                container.Clear();
                try
                {
                    StreamReader r = new StreamReader("task.txt");
                    {
                        while (!r.EndOfStream)
                        {
                            string l = r.ReadLine();
                            string[] parts = l.Split('|');

                            if (parts.Length != 6)
                            {
                                Console.WriteLine("Error Loading File!");
                                return;
                            }
                            TaskContainer task = new TaskContainer
                            {
                                title = parts[0],
                                description = parts[1],
                                deadline = DateTime.Parse(parts[2]),
                                priority = int.Parse(parts[3]),
                                category = parts[4],
                                isCompleted = bool.Parse(parts[5]),
                            };
                            container.Add(task);
                        }
                    }
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Task Loaded");
                    Console.ResetColor();
                }

                catch (IOException ex)
                {
                    Console.WriteLine("Error Loading Saved Data: "+ex.Message);
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Saved Data not Found");
                Console.ResetColor();
            }
        }
        //save our task to a txt file
        static void SaveTask()
        {
            try
            {
                StreamWriter w = new StreamWriter("task.txt");

                foreach (var item in container)
                {
                    w.WriteLine($"(item.title)| (item.description) | (item.category) | (item.priority)");
                }
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Task Saved Successfully");
                Console.ResetColor();
            }
            catch (IOException)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Error saving file");
                Console.ResetColor();
            }
            
        }
        //Adding the task function
        static void AddTask()
        {
            TaskContainer newTask = new TaskContainer();
            Console.Write("Enter Task Title: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow; //change input color
            newTask.title = Console.ReadLine(); //read the inputed title
            Console.ResetColor(); //reset color

            Console.Write("Enter Task Description: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            newTask.description = Console.ReadLine(); //read description color
            Console.ResetColor();

            Console.Write("Enter Task Deadline(yyy-mm-dd): ");
            DateTime dateTime;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            while (!DateTime.TryParse(Console.ReadLine(), out dateTime)) //make sure we put the correct date format
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Invalid Date Format, enter again in yyyy-mm-dd");
                Console.ResetColor();
            }
            Console.ResetColor();
            newTask.deadline = dateTime;

            Console.Write("Enter Task Priority(1 for High, 2 for Mid, 3 for Low): ");
            int prior;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            while (!int.TryParse(Console.ReadLine(), out prior)) //make sure we put an integer
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Invalid Priority Format, enter an integer");
                Console.ResetColor();
            }
            Console.ResetColor();
            newTask.priority = prior;

            Console.WriteLine("Enter Category: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            newTask.category = Console.ReadLine();
            Console.ResetColor();

            container.Add(newTask);
        }
        //view the task function
        static void ViewTask()
        {
            //we get all the items in our class items
            foreach (var taskList in container)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"Title: {taskList.title}, Description: {taskList.description} \nDeadline: {taskList.category}, Priority: {taskList.priority}");
                Console.ResetColor();
            }
        }
        //editing the task function
        static void EditTask()
        {
            ViewTask(); //we first view the task
            Console.WriteLine("Enter the Title of the task to Edit: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            string editTitle = Console.ReadLine();
            Console.ResetColor();

            TaskContainer taskListEdit = container.FirstOrDefault(t => t.title == editTitle);
            if(editTitle ==null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("No Task Found");
                Console.ResetColor();
                return;
            }

            Console.WriteLine("Enter new Description or press Enter key to keep current: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            string newDes = Console.ReadLine();
            Console.ResetColor();
            if(!string.IsNullOrEmpty(newDes))
            {
                taskListEdit.description = newDes;
            }

            Console.WriteLine("Enter new Deadline(yyy-mm-dd) or press Enter key to keep current: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            string newDead = Console.ReadLine();
            Console.ResetColor();
            DateTime deadLine;
            if(DateTime.TryParse(newDead, out deadLine))
            {
                taskListEdit.deadline = deadLine;
            }

            Console.WriteLine("Enter new Priority(1, 2 or 3) or press Enter key to keep current: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            string newPrior = Console.ReadLine();
            Console.ResetColor();
            int prior;
            if(int.TryParse(newPrior, out prior))
            {
                taskListEdit.priority = prior;
            }

            Console.WriteLine("Enter new Category or press Enter key to keep current: ");
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            string newCat = Console.ReadLine();
            Console.ResetColor();
            if(!string.IsNullOrEmpty(newCat))
            {
                taskListEdit.category = newCat;
            }
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Task Edit : Succesful ");
            Console.ResetColor();
        }
        static void TaskCompleted()
        {
            ViewTask();

            Console.WriteLine("TEnter the Title of the task to Mark Completed ");
            string titleToMark = Console.ReadLine();

            TaskContainer titleContainer = container.FirstOrDefault(t => t.title == titleToMark);

            if(titleContainer ==null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("No Task Found");
                Console.ResetColor();
                return;
            }
            titleContainer.isCompleted = true;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("Task Marked Completed");
            Console.ResetColor();
        }
        //filtering the task function
        static void FilterTask()
        {
            Console.WriteLine("Filter by :\n#1. Category \n#2. Priority \n#3. Completed Status");
            int selected = int.Parse(Console.ReadLine());
            IEnumerable<TaskContainer> filterList = new List<TaskContainer>();
            //read the input to determine which parameter to filter by
            switch (selected)
            {
                case 1:
                    Console.WriteLine("Enter Category");
                    string category = Console.ReadLine();
                    filterList = container.Where(t => t.category == category);
                    break;
                case 2:
                    Console.WriteLine("Enter Priority(1, 2 or 3)");
                    string priority = Console.ReadLine();
                    filterList = container.Where(t => t.category == priority);
                    break;
                case 3:
                    Console.WriteLine("Enter Completed Status(1 : Completed, 2 : NotCompleted");
                    int stat = int.Parse(Console.ReadLine());
                    bool isCompleted = stat == 1;
                    filterList = container.Where(t => t.isCompleted == isCompleted);
                    break;
                default:
                    Console.WriteLine("Wrong Input! Enter 1 , 2 or 3");
                    break;
            }
            //get the items in the filterList and display them
            foreach (var item in filterList)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"Title: {item.title}, Description: {item.description} Deadline: {item.category}, Priority: {item.priority}");
                Console.ResetColor();
            }
        }
        //sorting the task function
        static void SortTask()
        {
            Console.WriteLine("Sort by: \n#1. Titles \n#2. Deadline \n#3. Priority");
            int selected = int.Parse(Console.ReadLine());
            IEnumerable<TaskContainer> sortTask = new List<TaskContainer>();
            //read the input to determine which parameter to sort by
            switch (selected)
            {
                case 1:
                    sortTask = container.OrderBy(t => t.title);
                    break;
                case 2:
                    sortTask = container.OrderBy(t => t.deadline);
                    break;
                case 3:
                    sortTask = container.OrderBy(t => t.priority);
                    break;
                default:
                    break;
            }
            foreach (var item in sortTask)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine($"Title: {item.title}, Description: {item.description}, Deadline: {item.category}, Priority: {item.priority}");
                Console.ResetColor();
            }
        }
        static void RemoveTask()
        {
            ViewTask();

            Console.WriteLine("Enter the title of the task to Delete");
            string titleDelete = Console.ReadLine();

            TaskContainer taskDelete = container.FirstOrDefault(t => t.title == titleDelete);
            if(taskDelete ==null)
            {
                Console.WriteLine("No Task Found");
            }
            container.Remove(taskDelete);
            Console.WriteLine("Task Deleted!");
        }
        //
    }
}

