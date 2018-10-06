add-migration InitialCreate -Project 'PumoxTest.DataBase' -Context PumoxTest.DataBase.PumoxTestContext
remove-migration -Project 'PumoxTest.DataBase' -Context PumoxTest.DataBase.PumoxTestContext

Tworzenie bazy danych z lini komend konsoli 'Package Manager':
update-database -Project 'PumoxTest.DataBase' -Context PumoxTest.DataBase.PumoxTestContext

INFO:
Przed uruchomieniem należy zmienić wpis: 
    "DefaultConnection": "Server=.;Database=PumoxTest;Integrated Security=True;MultipleActiveResultSets=True;"
w pliku appsettings.json jeśli jest taka potrzeba.

Baza danych jest podnoszona do najnowszej migracji i uzupełniana przykładowymi danymi automatycznie przy każdym uruchomieniu.

BasicAuthentication: 
user i password zdefiniowane bezpośrednio w AuthenticationEvents