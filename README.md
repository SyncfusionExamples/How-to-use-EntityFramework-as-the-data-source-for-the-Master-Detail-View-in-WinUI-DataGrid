# How to use EntityFramework as the data source for the Master-Detail View in WinUI DataGrid

In [WinUI DataGrid](https://help.syncfusion.com/cr/winui/Syncfusion.UI.Xaml.DataGrid.SfDataGrid.html) (SfDataGrid), there was a way to utilize `EntityFramework` as the source for the  [MasterDetailsView](https://help.syncfusion.com/winui/datagrid/master-details-view). This process can be achieved by creating a class from `DbContext` and setting up `SQLite` as the database provider with the connection string. Additionally, the data for the `MasterDetailsView` is loaded and the SaveChanges method from `DbContext` is used to persist any changes made in the current context to the underlying database.
 ```C#
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
 ```
