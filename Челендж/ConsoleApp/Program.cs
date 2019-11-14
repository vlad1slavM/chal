using Challenge;
using Challenge.DataContracts;
using Polynoms;
using System;
using Z.Expressions;
using System.Globalization;

namespace ConsoleApp
{
    class Program
    {
        public static object GlobalObject { get; private set; }

        static void Main(string[] args)
        {
            const string teamSecret = "xCtZumI4Duc1ROdh6orB9Aip0YkMz9gr";
            if (string.IsNullOrEmpty(teamSecret))
            {
                Console.WriteLine("Задай секрет своей команды, чтобы можно было делать запросы от ее имени");
                return;
            }
            var challengeClient = new ChallengeClient(teamSecret);

            const string challengeId = "projects-course";
            Console.ReadLine();
            var challenge = challengeClient.GetChallengeAsync(challengeId).Result;
            Console.WriteLine(challenge.Description);
            Console.WriteLine();
            Console.WriteLine("----------------");
            Console.WriteLine();

            Console.WriteLine($"Нажми ВВОД, чтобы получить список взятых командой задач");
            Console.ReadLine();
            var allTasks = challengeClient.GetAllTasksAsync().Result;
            for (int i = 0; i < allTasks.Count; i++)
            {
                var task = allTasks[i];
                Console.WriteLine($"  Задание {i + 1}, статус {task.Status}");
                Console.WriteLine($"  Формулировка: {task.UserHint}");
                Console.WriteLine($"                {task.Question}");
                Console.WriteLine();
            }
            Console.WriteLine("----------------");
            Console.WriteLine();

            string type = "polynomial-root";
            string round = challenge.Rounds[challenge.Rounds.Count - 1].Id;
            Console.WriteLine($"Нажми ВВОД, чтобы получить задачу типа {type}");
            Console.ReadLine();
            for (var i = 0; i < 30; i++)
            {
                var newTask = challengeClient.AskNewTaskAsync(round, type).Result;
                /*Console.WriteLine($"  Новое задание, статус {newTask.Status}");
                Console.WriteLine($"  Формулировка: {newTask.UserHint}");
                Console.WriteLine($"                {newTask.Question}");
                Console.WriteLine();
                Console.WriteLine("----------------");
                Console.WriteLine();*/

                string answer = Cuesor.GetRoot(newTask.Question);
                //Console.WriteLine($"Нажми ВВОД, чтобы ответить на полученную задачу самым правильным ответом: {answer}");
                
                var updatedTask = challengeClient.CheckTaskAnswerAsync(newTask.Id, answer).Result;

                Console.WriteLine($"  Новое задание, статус {updatedTask.Status}");
                Console.WriteLine($"  Формулировка:  {updatedTask.UserHint}");
                Console.WriteLine($"                 {updatedTask.Question}");
                Console.WriteLine($"  Ответ команды: {updatedTask.TeamAnswer}");
                Console.WriteLine();
                if (updatedTask.Status == TaskStatus.Success)
                    Console.WriteLine($"Ура! Ответ угадан!");
                else if (updatedTask.Status == TaskStatus.Failed)
                    Console.WriteLine($"Похоже ответ не подошел и задачу больше сдать нельзя...");
                Console.WriteLine();
                Console.WriteLine("----------------");
                Console.WriteLine();
                if (updatedTask.Status == TaskStatus.Failed) break;
            }

            Console.WriteLine($"Нажми ВВОД, чтобы завершить работу программы");
            Console.ReadLine();
        }
    }
}
