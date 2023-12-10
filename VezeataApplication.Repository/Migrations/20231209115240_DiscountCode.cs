using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VezeataApplication.Repository.Migrations
{
    /// <inheritdoc />
    public partial class DiscountCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_DiscountCodeCoupon_CouponId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCodeCouponVezeataUser_DiscountCodeCoupon_DiscountCodesId",
                table: "DiscountCodeCouponVezeataUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiscountCodeCoupon",
                table: "DiscountCodeCoupon");

            migrationBuilder.RenameTable(
                name: "DiscountCodeCoupon",
                newName: "DiscountCodeCoupons");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiscountCodeCoupons",
                table: "DiscountCodeCoupons",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_DiscountCodeCoupons_CouponId",
                table: "Bookings",
                column: "CouponId",
                principalTable: "DiscountCodeCoupons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCodeCouponVezeataUser_DiscountCodeCoupons_DiscountCodesId",
                table: "DiscountCodeCouponVezeataUser",
                column: "DiscountCodesId",
                principalTable: "DiscountCodeCoupons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_DiscountCodeCoupons_CouponId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCodeCouponVezeataUser_DiscountCodeCoupons_DiscountCodesId",
                table: "DiscountCodeCouponVezeataUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiscountCodeCoupons",
                table: "DiscountCodeCoupons");

            migrationBuilder.RenameTable(
                name: "DiscountCodeCoupons",
                newName: "DiscountCodeCoupon");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiscountCodeCoupon",
                table: "DiscountCodeCoupon",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_DiscountCodeCoupon_CouponId",
                table: "Bookings",
                column: "CouponId",
                principalTable: "DiscountCodeCoupon",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCodeCouponVezeataUser_DiscountCodeCoupon_DiscountCodesId",
                table: "DiscountCodeCouponVezeataUser",
                column: "DiscountCodesId",
                principalTable: "DiscountCodeCoupon",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
