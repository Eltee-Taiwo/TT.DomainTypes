# TT.DomainTypes
A record based solution to the primitive type obsession problem


## The problem:

We use primitive types like strings and integers everywhere and we occassionally run into issues where the wrong string variable is passed into a function.

```csharp
public static string GetExistingUserName(string userName)
{
    ...
}

```
However this could be accidentally used as so:

```csharp
public static void Main()
{
    string userName = ReadFromPrivateFunction();
    string  favouriteFood = ReadFromOtherPrivateFunction();

    var existingUserName = GetExistingUserName(favouriteFood); //Nothing stops us from doing this
}
```

However, if we were to use a domain type, we would get a compile type error preventing us from making such a mistake

```csharp
public record UserName(string Value) : DomainTypeString(Value);

public record Food(string Value) : DomainTypeString(Value);
```

Now that we have the Domain Types, that are backed by the record reference type, we can now get intellisense and compile errors.

Now our functions become

```csharp
public static UserName GetExistingUserName(UserName userName)
{
    ...
}

public static void Main()
{
    UserName userName = ReadFromPrivateFunction();
    Food  favouriteFood = ReadFromOtherPrivateFunction();

    var existingUserName = GetExistingUserName(favouriteFood); //Compile time error here because we cannot assign a Food domain type to a UserName domain type
}
```

We also get the benefit of intellisense which we help us autocomplete or recommend local variables that match the required domain type.