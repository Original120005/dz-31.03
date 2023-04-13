using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace dz_31_03
{
    class Originator
    {
        private string state;
        public Originator(string state)
        {
            this.state = state;
        }

        public void CreateState()
        {
            Console.WriteLine("Originator: I'm doing something important.");
            this.state = GenerateRandomString(30);
            Console.WriteLine($"Originator: and my state has changed to: {state}");
        }
        private string GenerateRandomString(int length = 10)
        {
            string allowedSymbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string result = string.Empty;
            while (length > 0)
            {
                result += allowedSymbols[new Random().Next(0, allowedSymbols.Length)];
                Thread.Sleep(12);
                length--;
            }
            return result;
        }

        public IMemento Save()
        {
            return new ConcreteMemento(state);
        }
        public void Restore(IMemento memento)
        {
            if (!(memento is ConcreteMemento))
            {
                throw new Exception("Unknown memento class " + memento.ToString());
            }
            this.state = memento.GetState();
            Console.Write($"Originator: My state has changed to: {state}");
        }
    }


    public interface IMemento
    {
        string GetName();
        string GetState();
        DateTime GetDate();
    }

    class ConcreteMemento : IMemento
    {
        private string state;
        private DateTime date;

        public ConcreteMemento(string state)
        {
            this.state = state;
            date = DateTime.Now;
        }

        public string GetState()
        {
            return state;
        }
        public string GetName()
        {
            return $"{date} / ({state.Substring(0, 9)})...";
        }
        public DateTime GetDate()
        {
            return date;
        }
    }

    class Caretaker
    {
        private List<IMemento> mementos = new List<IMemento>();
        private Originator originator = null;

        public Caretaker(Originator originator)
        {
            this.originator = originator;
        }

        public void Backup()
        {
            Console.WriteLine("\nCaretaker: Saving Originator's state...");
            mementos.Add(originator.Save());
        }

        public void Undo()
        {
            if (mementos.Count == 0)
            {
                return;
            }
            var memento = mementos.Last();
            mementos.Remove(memento);
            Console.WriteLine("Caretaker: Restoring state to: " + memento.GetName());

            try
            {
                originator.Restore(memento);
            }
            catch (Exception)
            {
                Undo();
            }
        }

        public void ShowHistory()
        {
            Console.WriteLine("Caretaker: Here's the list of mementos:");
            foreach (var memento in mementos)
            {
                Console.WriteLine(memento.GetName());
            }
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            Originator originator = new Originator("Super-duper-super-puper-super.");
            Caretaker caretaker = new Caretaker(originator);

            caretaker.Backup();
            originator.CreateState();

            caretaker.Backup();
            originator.CreateState();

            caretaker.Backup();
            originator.CreateState();

            Console.WriteLine();
            caretaker.ShowHistory();

            Console.WriteLine("\nClient: Now, let's rollback!\n");
            caretaker.Undo();

            Console.WriteLine("\n\nClient: Once more!\n");
            caretaker.Undo();

            Console.WriteLine();


        }
    }
}