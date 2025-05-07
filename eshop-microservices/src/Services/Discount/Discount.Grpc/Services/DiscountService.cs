using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService(
    DiscountContext dbContext, 
    ILogger<DiscountService> logger) 
    : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext
                            .Coupons
                            .FirstOrDefaultAsync(x => x.ProductName == request.ProductName);
        
        if (coupon is null)
        {
            coupon = new Coupon
            {
                ProductName = "No Discount",
                Amount = 0,
                Description = "No Discount Desc"
            };
        }
        logger.LogInformation("Discount is retrieved for the product: {ProductName}, Amount: {Amount}", coupon.ProductName, coupon.Amount);

        var couponModel = coupon.Adapt<CouponModel>();

        return couponModel;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon is null)
        {
            var errDetails = "Invalid request object";
            logger.LogWarning("Request failed. StatusCode: {StatusCode}, Details: {Details}", StatusCode.InvalidArgument, errDetails);
            throw new RpcException(new Status(StatusCode.InvalidArgument, errDetails));
        }
        dbContext.Coupons.Add(coupon);

        await dbContext.SaveChangesAsync();

        logger.LogInformation("Coupon is successfully created. ProductName: {ProductName}, Amount: {Amount}", coupon.ProductName, coupon.Amount);

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        if (coupon is null)
        {
            var errDetails = "Invalid request object";
            logger.LogWarning("Request failed. StatusCode: {StatusCode}, Details: {Details}", StatusCode.InvalidArgument, errDetails);
            throw new RpcException(new Status(StatusCode.InvalidArgument, errDetails));
        }
        dbContext.Coupons.Update(coupon);

        await dbContext.SaveChangesAsync();

        logger.LogInformation("Coupon is successfully updated. ProductName: {ProductName}, Amount: {Amount}", coupon.ProductName, coupon.Amount);

        var couponModel = coupon.Adapt<CouponModel>();
        return couponModel;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = dbContext.Coupons.FirstOrDefault(x => x.ProductName == request.ProductName);
        if (coupon is null)
        {
            var errDetails = "Coupon not found";
            logger.LogWarning("Request failed. StatusCode: {StatusCode}, Details: {Details}", StatusCode.NotFound, errDetails);
            throw new RpcException(new Status(StatusCode.NotFound, errDetails));
        }

        dbContext.Coupons.Remove(coupon);

        await dbContext.SaveChangesAsync();

        logger.LogInformation("Coupon is successfully deleted. ProductName: {ProductName}, Amount: {Amount}", coupon.ProductName, coupon.Amount);

        return new DeleteDiscountResponse { Success = true };
    }
}
