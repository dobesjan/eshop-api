﻿// <auto-generated />
using System;
using Eshop.Api.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Eshop.Api.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Eshop.Api.Models.Contacts.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CountryId")
                        .HasColumnType("int");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Eshop.Api.Models.Contacts.Contact", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AddressId")
                        .HasColumnType("int");

                    b.Property<int?>("CustomerId")
                        .HasColumnType("int");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("CustomerId")
                        .IsUnique()
                        .HasFilter("[CustomerId] IS NOT NULL");

                    b.HasIndex("PersonId");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("Eshop.Api.Models.Contacts.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Acronym")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Country");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Acronym = "CZ",
                            IsEnabled = true,
                            Name = "Česká republika"
                        },
                        new
                        {
                            Id = 2,
                            Acronym = "SK",
                            IsEnabled = true,
                            Name = "Slovenská republika"
                        },
                        new
                        {
                            Id = 3,
                            Acronym = "USA",
                            IsEnabled = false,
                            Name = "Spojené státy americké"
                        });
                });

            modelBuilder.Entity("Eshop.Api.Models.Contacts.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ContactId")
                        .HasColumnType("int");

                    b.Property<bool>("IsLogged")
                        .HasColumnType("bit");

                    b.Property<bool>("NewsletterAgree")
                        .HasColumnType("bit");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Eshop.Api.Models.Contacts.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("Eshop.Api.Models.Currencies.Currency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Acronym")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Currencies");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Acronym = "Kč",
                            Name = "Česká koruna"
                        },
                        new
                        {
                            Id = 2,
                            Acronym = "€",
                            Name = "Euro"
                        });
                });

            modelBuilder.Entity("Eshop.Api.Models.Currencies.CurrencyPreference", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.ToTable("CurrencyPreferences");
                });

            modelBuilder.Entity("Eshop.Api.Models.Images.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ImageGroupId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ImageGroupId");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("Eshop.Api.Models.Images.ImageGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ImageGroup");
                });

            modelBuilder.Entity("Eshop.Api.Models.Orders.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AddressId")
                        .HasColumnType("int");

                    b.Property<int?>("BillingContactId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<int?>("CustomerId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DeliveryTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsOrdered")
                        .HasColumnType("bit");

                    b.Property<int>("OrderStatusId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("SentTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ShippingId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.HasIndex("BillingContactId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("OrderStatusId");

                    b.HasIndex("ShippingId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Eshop.Api.Models.Orders.OrderProduct", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<double>("Cost")
                        .HasColumnType("float");

                    b.Property<double>("CostBefore")
                        .HasColumnType("float");

                    b.Property<double>("CostWithTax")
                        .HasColumnType("float");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.HasKey("ProductId", "OrderId");

                    b.HasIndex("OrderId");

                    b.ToTable("OrderProducts");
                });

            modelBuilder.Entity("Eshop.Api.Models.Orders.OrderStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("OrderStatus");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Address filled"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Shipping filled"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Payment method filled"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Order sent"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Order in delivery service"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Order processed"
                        });
                });

            modelBuilder.Entity("Eshop.Api.Models.Orders.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Cost")
                        .HasColumnType("float");

                    b.Property<double>("CostWithTax")
                        .HasColumnType("float");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<int>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("PaymentMethodId")
                        .HasColumnType("int");

                    b.Property<int>("PaymentStatusId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("OrderId")
                        .IsUnique();

                    b.HasIndex("PaymentMethodId");

                    b.HasIndex("PaymentStatusId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("Eshop.Api.Models.Orders.PaymentMethod", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PaymentMethods");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Enabled = true,
                            Name = "Cash on delivery"
                        },
                        new
                        {
                            Id = 2,
                            Enabled = true,
                            Name = "Card"
                        });
                });

            modelBuilder.Entity("Eshop.Api.Models.Orders.PaymentStatus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PaymentStatuses");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Not paid"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Paid"
                        });
                });

            modelBuilder.Entity("Eshop.Api.Models.Orders.Shipping", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Shippings");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Enabled = true,
                            Name = "DPD"
                        },
                        new
                        {
                            Id = 2,
                            Enabled = true,
                            Name = "PPL"
                        });
                });

            modelBuilder.Entity("Eshop.Api.Models.Orders.ShippingPaymentMethod", b =>
                {
                    b.Property<int>("PaymentMethodId")
                        .HasColumnType("int");

                    b.Property<int>("ShippingId")
                        .HasColumnType("int");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.HasKey("PaymentMethodId", "ShippingId");

                    b.HasIndex("ShippingId");

                    b.ToTable("ShippingPaymentMethods");

                    b.HasData(
                        new
                        {
                            PaymentMethodId = 1,
                            ShippingId = 1,
                            Id = 1
                        },
                        new
                        {
                            PaymentMethodId = 2,
                            ShippingId = 1,
                            Id = 2
                        },
                        new
                        {
                            PaymentMethodId = 1,
                            ShippingId = 2,
                            Id = 3
                        });
                });

            modelBuilder.Entity("Eshop.Api.Models.Products.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ParentCategoryId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParentCategoryId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Eshop.Api.Models.Products.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BuyLimit")
                        .HasColumnType("int");

                    b.Property<bool>("Enabled")
                        .HasColumnType("bit");

                    b.Property<bool>("IsInStock")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ViewOrder")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Eshop.Api.Models.Products.ProductCategory", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.HasKey("ProductId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("ProductCategories");
                });

            modelBuilder.Entity("Eshop.Api.Models.Products.ProductImage", b =>
                {
                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("ImageId")
                        .HasColumnType("int");

                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.HasKey("ProductId", "ImageId");

                    b.HasIndex("ImageId");

                    b.ToTable("ProductImage");
                });

            modelBuilder.Entity("Eshop.Api.Models.Products.ProductPriceList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Cost")
                        .HasColumnType("float");

                    b.Property<double>("CostBefore")
                        .HasColumnType("float");

                    b.Property<double>("CostWithTax")
                        .HasColumnType("float");

                    b.Property<int>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductPrices");
                });

            modelBuilder.Entity("Eshop.Api.Models.Contacts.Address", b =>
                {
                    b.HasOne("Eshop.Api.Models.Contacts.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Country");
                });

            modelBuilder.Entity("Eshop.Api.Models.Contacts.Contact", b =>
                {
                    b.HasOne("Eshop.Api.Models.Contacts.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId");

                    b.HasOne("Eshop.Api.Models.Contacts.Customer", "Customer")
                        .WithOne("Contact")
                        .HasForeignKey("Eshop.Api.Models.Contacts.Contact", "CustomerId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Eshop.Api.Models.Contacts.Person", "Person")
                        .WithMany()
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");

                    b.Navigation("Customer");

                    b.Navigation("Person");
                });

            modelBuilder.Entity("Eshop.Api.Models.Currencies.CurrencyPreference", b =>
                {
                    b.HasOne("Eshop.Api.Models.Currencies.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Currency");
                });

            modelBuilder.Entity("Eshop.Api.Models.Images.Image", b =>
                {
                    b.HasOne("Eshop.Api.Models.Images.ImageGroup", "ImageGroup")
                        .WithMany()
                        .HasForeignKey("ImageGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ImageGroup");
                });

            modelBuilder.Entity("Eshop.Api.Models.Orders.Order", b =>
                {
                    b.HasOne("Eshop.Api.Models.Contacts.Address", "DeliveryAddress")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Eshop.Api.Models.Contacts.Contact", "BillingContact")
                        .WithMany()
                        .HasForeignKey("BillingContactId");

                    b.HasOne("Eshop.Api.Models.Currencies.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Eshop.Api.Models.Contacts.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId");

                    b.HasOne("Eshop.Api.Models.Orders.OrderStatus", "OrderStatus")
                        .WithMany()
                        .HasForeignKey("OrderStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Eshop.Api.Models.Orders.Shipping", "Shipping")
                        .WithMany()
                        .HasForeignKey("ShippingId");

                    b.Navigation("BillingContact");

                    b.Navigation("Currency");

                    b.Navigation("Customer");

                    b.Navigation("DeliveryAddress");

                    b.Navigation("OrderStatus");

                    b.Navigation("Shipping");
                });

            modelBuilder.Entity("Eshop.Api.Models.Orders.OrderProduct", b =>
                {
                    b.HasOne("Eshop.Api.Models.Orders.Order", "Order")
                        .WithMany("OrderProducts")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Eshop.Api.Models.Products.Product", "Product")
                        .WithMany("OrderProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Eshop.Api.Models.Orders.Payment", b =>
                {
                    b.HasOne("Eshop.Api.Models.Currencies.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Eshop.Api.Models.Orders.Order", "Order")
                        .WithOne("Payment")
                        .HasForeignKey("Eshop.Api.Models.Orders.Payment", "OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Eshop.Api.Models.Orders.PaymentMethod", "PaymentMethod")
                        .WithMany()
                        .HasForeignKey("PaymentMethodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Eshop.Api.Models.Orders.PaymentStatus", "PaymentStatus")
                        .WithMany()
                        .HasForeignKey("PaymentStatusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Currency");

                    b.Navigation("Order");

                    b.Navigation("PaymentMethod");

                    b.Navigation("PaymentStatus");
                });

            modelBuilder.Entity("Eshop.Api.Models.Orders.ShippingPaymentMethod", b =>
                {
                    b.HasOne("Eshop.Api.Models.Orders.PaymentMethod", "PaymentMethod")
                        .WithMany("ShippingPaymentMethod")
                        .HasForeignKey("PaymentMethodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Eshop.Api.Models.Orders.Shipping", "Shipping")
                        .WithMany("ShippingPaymentMethod")
                        .HasForeignKey("ShippingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PaymentMethod");

                    b.Navigation("Shipping");
                });

            modelBuilder.Entity("Eshop.Api.Models.Products.Category", b =>
                {
                    b.HasOne("Eshop.Api.Models.Products.Category", "ParentCategory")
                        .WithMany()
                        .HasForeignKey("ParentCategoryId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("ParentCategory");
                });

            modelBuilder.Entity("Eshop.Api.Models.Products.ProductCategory", b =>
                {
                    b.HasOne("Eshop.Api.Models.Products.Category", "Category")
                        .WithMany("ProductCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Eshop.Api.Models.Products.Product", "Product")
                        .WithMany("ProductCategories")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Eshop.Api.Models.Products.ProductImage", b =>
                {
                    b.HasOne("Eshop.Api.Models.Images.Image", "Image")
                        .WithMany("ProductImages")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Eshop.Api.Models.Products.Product", "Product")
                        .WithMany("ProductImages")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Image");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Eshop.Api.Models.Products.ProductPriceList", b =>
                {
                    b.HasOne("Eshop.Api.Models.Currencies.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Eshop.Api.Models.Products.Product", "Product")
                        .WithMany("ProductPrices")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Currency");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Eshop.Api.Models.Contacts.Customer", b =>
                {
                    b.Navigation("Contact");
                });

            modelBuilder.Entity("Eshop.Api.Models.Images.Image", b =>
                {
                    b.Navigation("ProductImages");
                });

            modelBuilder.Entity("Eshop.Api.Models.Orders.Order", b =>
                {
                    b.Navigation("OrderProducts");

                    b.Navigation("Payment");
                });

            modelBuilder.Entity("Eshop.Api.Models.Orders.PaymentMethod", b =>
                {
                    b.Navigation("ShippingPaymentMethod");
                });

            modelBuilder.Entity("Eshop.Api.Models.Orders.Shipping", b =>
                {
                    b.Navigation("ShippingPaymentMethod");
                });

            modelBuilder.Entity("Eshop.Api.Models.Products.Category", b =>
                {
                    b.Navigation("ProductCategories");
                });

            modelBuilder.Entity("Eshop.Api.Models.Products.Product", b =>
                {
                    b.Navigation("OrderProducts");

                    b.Navigation("ProductCategories");

                    b.Navigation("ProductImages");

                    b.Navigation("ProductPrices");
                });
#pragma warning restore 612, 618
        }
    }
}
