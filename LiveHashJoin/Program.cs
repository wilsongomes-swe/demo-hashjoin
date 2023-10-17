
// SELECT O.OrderId, O.Item, C.Name as CustomerName FROM order as O INNER JOIN customer as C ON O.CustomerId = C.CustomerId OPTION (HASH JOIN)

Console.WriteLine("Hello, World!");

var orderLines = new List<Order>
{
    new(1, "Keyboard", 4),
    new(2, "Computer", 3),
    new(3, "Mousepad", 1),
    new(4, "Notebook", 4)
};

var customerLines = new List<Customer>
{
    new(1, "Thiago TC"),
    new(2, "Wilson"),
    new(3, "Daniel"),
    new(4, "Hercules")
};

var result = JoingAlgorithms.HashJoin(orderLines, customerLines);
foreach (var resultLine in result)
{
    Console.WriteLine(resultLine);
}

record Order(int OrderId, string Item, int CustomerId);
record Customer(int CustomerId, string Name);

record QueryResult(int OrderId, string Item, string CustomerName);


static class JoingAlgorithms
{
    internal static IReadOnlyList<QueryResult> HashJoin(IReadOnlyList<Order> orders, IReadOnlyList<Customer> customers)
    {
        // build
        var customersHashtable = new Dictionary<int, List<Customer>>();
        foreach (var customer in customers)
        {
            if(customersHashtable.ContainsKey(customer.CustomerId))
                customersHashtable[customer.CustomerId].Add(customer);    
            else
                customersHashtable.Add(customer.CustomerId, new() { customer });
        }
        
        // probe
        var results = new List<QueryResult>();
        foreach (var order in orders)
        {
            if (customersHashtable.TryGetValue(order.CustomerId, out var matchList))
            {
                foreach (var customer in matchList)
                {
                    results.Add(new(order.OrderId, order.Item, customer.Name));
                }   
            }
        }
        
        return results;
    }
}