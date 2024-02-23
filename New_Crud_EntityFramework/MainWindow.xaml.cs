using Microsoft.EntityFrameworkCore;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Syncfusion.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace New_Crud_EntityFramework
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        private PeopleViewModel ViewModel { get; } = new PeopleViewModel();
        public List<Person> People => ViewModel.GetPeople();

        public List<Address> Addresses => ViewModel.GetAddresses();
        public MainWindow()
        {
            this.InitializeComponent();
            var newPerson1 = new Person {Id=1000, Name = "John Doe", Age = 30 , PersonDetailsAddress=new ObservableCollection<Address>() { new Address { Id = 1000, Street = "A", City = "chennai" } } };
            ViewModel.AddPerson(newPerson1);
            var newPerson2 = new Person {Id=2000,Name = "Doe", Age = 20 , PersonDetailsAddress = new ObservableCollection<Address>() { new Address { Id = 2000, Street = "B", City = "Tanjore" } } };
            ViewModel.AddPerson(newPerson2);
            this.dataGrid.CurrentCellEndEdit += DataGrid_CurrentCellEndEdit;
            this.dataGrid.RecordDeleting += DataGrid_RecordDeleting;
            this.dataGrid.Loaded += DataGrid_Loaded;
        }

        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            this.dataGrid.View.Records.CollectionChanged += Records_CollectionChanged;
        }

        private void Records_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Here we can save the data from the 
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                // Added the data to the DbSet and updated to database.
                ViewModel._context.People.Add((e.NewItems[0] as RecordEntry).Data as Person);
                ViewModel._context.SaveChanges();
            }
        }

        private void DataGrid_RecordDeleting(object sender, Syncfusion.UI.Xaml.DataGrid.RecordDeletingEventArgs e)
        {
            var selectedPerson = dataGrid.SelectedItem as Person;
            if (selectedPerson != null)
            {
                // Delete the data from the DbSet and updated to database.
                ViewModel._context.People.Remove(selectedPerson);
                ViewModel._context.SaveChanges();
            }
        }

        private void DataGrid_CurrentCellEndEdit(object sender, Syncfusion.UI.Xaml.DataGrid.CurrentCellEndEditEventArgs e)
        {
            var selectedPerson = dataGrid.SelectedItem as Person;
            if (selectedPerson != null)
            {
                // Update the data from the DbSet and updated to database.
                ViewModel._context.People.Update(selectedPerson);
                ViewModel._context.SaveChanges();
            }
        }
    }

    // Create a data model class representing the entity you want to CRUD
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public ObservableCollection<Address> PersonDetailsAddress { get; set; }
    }

    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        // Add other address-related properties as needed

    }

    // Create a DbContext class that represents your database context.
    public class AppDbContext : DbContext
    {
        public DbSet<Person> People { get; set; }

        public DbSet<Address> PeopleAddress { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Set up SQLite as the database provider with the connection string.
            string connectionString = "Data Source=PeopleDatabase.db";
            optionsBuilder.UseSqlite(connectionString);
        }
    }

    public class PeopleViewModel
    {
        public readonly AppDbContext _context = new AppDbContext();
        public List<Person> GetPeople()
        {
            // Read data 
            return _context.People.ToList<Person>();
        }
        public List<Address> GetAddresses()
        {
            return _context.PeopleAddress.ToList();
        }

        public void AddPerson(Person person)
        {
            // Add a new person to the People DbSet
            _context.People.Add(person);
            // Save changes to the database
        } 
    }
}
