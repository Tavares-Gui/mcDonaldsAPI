using System;
using System.Data;
using System.Linq;
using McDonaldsAPI.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace McDonaldsAPI.Services;

public class OrderRepository : IOrderRepository
{
    private readonly McDataBaseContext ctx;

    public OrderRepository(McDataBaseContext ctx)
        => this.ctx = ctx;

     public async Task<int> CreateOder(int storeId)
    {
        var selectedStore =
            from store in ctx.Stores
            where store.Id == storeId
            select store;
        if(!selectedStore.Any())
            throw new Exception("Store don't exist.");
        
        var clientOrder  = new ClientOrder();
        
        clientOrder.StoreId = storeId;
        clientOrder.OrderCode = "abcd1234";

        ctx.Add(clientOrder);
        await ctx.SaveChangesAsync();

        return clientOrder.Id;
    }

    public async Task CancelOrder(int orderId)
    {
        var currentOrder = await getOrder(orderId);

        if(currentOrder is null)
            throw new Exception("the order don't exist.");

        ctx.Remove(currentOrder);
        await ctx.SaveChangesAsync();
    }

    public async Task AddItem(int orderId, int productId)
    {
        var order = await getOrder(orderId);

        if(order is null)
            throw new Exception("the order don't exist.");

        var products = 
            from product in ctx.MenuItems
            where product.Id == productId
            select product;

        var selectedProduct = await products
            .FirstOrDefaultAsync();

        if(selectedProduct is null)
            throw new Exception("Product don't exist.");

        var item = new ClientOrderItem();
        item.ClientOrderId = orderId;
        item.ProductId = orderId;

        ctx.Add(item);
        await ctx.SaveChangesAsync();
    }

    public Task RemoveItem(int orderId, int productId)
    {
        throw new System.NotImplementedException();
    }

    public Task<List<Product>> GetMenu(int orderId)
    {
        throw new System.NotImplementedException();
    }

    public Task<List<Product>> GetOrderItems(int orderId)
    {
        throw new System.NotImplementedException();
    }

    public Task DeliveryOrder(int orderId)
    {
        throw new System.NotImplementedException();
    }

    public Task FinishOrder(int orderId)
    {
        throw new System.NotImplementedException();
    }

    private async Task<ClientOrder> getOrder(int orderId)
    {
        var orders =
            from order in ctx.ClientOrders
            where order.Id == orderId
            select order;

        return await orders.FirstOrDefaultAsync();
    }
}