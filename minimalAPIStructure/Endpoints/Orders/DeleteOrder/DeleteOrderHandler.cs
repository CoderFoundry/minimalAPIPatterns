using Microsoft.AspNetCore.Http.HttpResults;
using MinimalAPIStructure.Data;

namespace minimalAPIStructure.Endpoints.Orders.DeleteOrder
{
    using DeleteOrderResult = Results<NotFound, NoContent>;

    public static class DeleteOrderHandler
    {
      public static async Task<DeleteOrderResult> Handle(int id, ApplicationDbContext db)
        {
            var order = await db.Orders.FindAsync(id);
            if (order is null)
            {
                return TypedResults.NotFound();
            }

            db.Orders.Remove(order);
            await db.SaveChangesAsync();
            
            return TypedResults.NoContent();
        }
    }
}
