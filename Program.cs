using System;

namespace Model_1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Введите количество работников");
            int staff = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Введите количество задач");
            int tasks = Convert.ToInt32(Console.ReadLine());

            double[,] task_time = new double[2, tasks]; //длительность задач
            double Topt = 0; //Подсчет частного суммы длительности и кол-ва работников
                             //После рекурсивной обработки массива будет равным оптимальному времени выполнения 

            //Ввод длительности задач
            for (int i = 0; i < tasks; i++)
            {
                Console.WriteLine($"Введите длительности {i + 1} работы");
                task_time[0, i] = i + 1;
                task_time[1, i] = Convert.ToDouble(Console.ReadLine());
                Topt += task_time[1, i];
            }

            Topt /= staff;
            int WorkerCounter = 0; //Счетчик занятых работников
            string[] result = new string[staff]; //Строковый массив для вывода диаграммы Ганта
            
            Count_Topt(ref staff, ref task_time, ref Topt, ref WorkerCounter, ref result); //Рекурсивная обработка массива с подсчетом оптимального времени


            double[] staff_tasks = new double[staff];
            int staffTaskCounter = 0;
            int k = 0; // счетчик задания

            for (int i = WorkerCounter; i < result.Length ; i++) //Для каждого еще не занятого работника
            {
                result[i] += $"Работник {i + 1}: ";
                while (k != tasks && staff_tasks[staffTaskCounter] < Topt) //Добавляем задания работнику до тех пор, пока время не дойдет до оптимального
                {
                        if (staff_tasks[staffTaskCounter] + task_time[1, k] <= Topt) //Если работник успевает сделать все задание - отдаем его полностью
                        {
                            result[i] += $"от {staff_tasks[staffTaskCounter]} до {staff_tasks[staffTaskCounter] + task_time[1, k]} - {task_time[0, k]} работа | ";
                            staff_tasks[staffTaskCounter] += task_time[1, k];
                            k++;
                        }
                        else //Если работник успевает выполнить только часть задания - отдаем ему эту часть
                        {
                            double dop = Topt - staff_tasks[staffTaskCounter];
                            result[i] += $"от {staff_tasks[staffTaskCounter]} до {staff_tasks[staffTaskCounter] + dop} - {task_time[0, k]} работа | ";
                            staff_tasks[staffTaskCounter] += dop;
                            task_time[1, k] -= dop;
                        }
                }
                staffTaskCounter++; //Переключаемся на следующего работника
            }

            Console.WriteLine("\nДиаграмма Ганта");
            foreach (string x in result)
            {
                Console.WriteLine(x);
            }
        }

        static void Count_Topt(ref int staff, ref double[,] task_time, ref double Topt, ref int count, ref string[] resArr)
        {
            double max = 0;
            int position = 0;
            bool flag = true; //Удовлетворяет ли набор заданий условию для перехода к использованию ленточной стратегии?
            
            //Проверяем наличие в массиве элементов, превышающих значение частного
            for (int i = 0; i < task_time.GetLength(1); i++)
            {
                if (task_time[1, i] <= Topt) //Если их нет - завершаем работу функции и переходим к применению ленточной стратегии
                {

                }
                else flag = false;
                
                if (!flag) //Если такой элемент есть - отдаем его текущему работнику и выполняем проверку массива без этого элемента 
                {
                    //Поиск максимального значения в массиве и его позиции 
                    for (int j = 0; j < task_time.GetLength(1); j++)
                    {
                        if (task_time[1, j] > max)
                        {
                            max = task_time[1, j];
                            position = j;
                        }
                    }

                    resArr[count] += $"Работник {count + 1}: от 0 до {max} - {task_time[0, position]} работа |";
                    count++; //Увеличиваем счетчик занятых работников
                    staff--; //Уменьшаяем общее количество свободных работников
                    task_time = Delete_Job(task_time, position); //Удаляем информации об отданой задаче
                    
                    //Пересчитываем оптимальное время
                    Topt = 0;
                    for (int f = 0; f < task_time.GetLength(1); f++)
                    {
                        Topt += task_time[1, f];
                    }
                    Topt /= staff;
                    //Вызываем функцию для новых массива и оптимального времени
                    Count_Topt(ref staff, ref task_time, ref Topt, ref count, ref resArr);
                    break;
                }
            }
        }

        static double[,] Delete_Job(double[,] arr, int position)
        {
            int marker = 0;
            double[,] temp = new double[2, arr.GetLength(1) - 1];
            for (int i = 0; i < position; i++)
            {
                temp[0, i] = arr[0, marker];
                temp[1, i] = arr[1, marker];
                marker++;
            }
            marker++;
            for (int i = position; i < temp.GetLength(1); i++)
            {
                temp[0, i] = arr[0, marker];
                temp[1, i] = arr[1, marker];
                marker++;
            }
            return temp;
        }
    }
}
