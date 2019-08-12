select * from Customers
select * from Products
select * from Categories
select * from Locations
select * from Sellers
select * from Deliverers
select * from Login
select * from Orders
select * from OrderItems
select * from OrderItemProducts
select * from Customers

//insert into Categories values('electrnic', null)
//insert into Products values('Nkia-8', '2018-8core 16 rear cam', 'original',80000,null,124,4.1,121,5,null,1,1)
//insert into Orders values (GetDate(), 1, 1, 'to be confirmed')
//insert into OrderItems values ( 10, 5 )
insert into OrderItems values ( 5, 5 )
//insert into OrderItemProducts values (GetDate(),4,2)

update Orders set Status='to be confirmed' where Id=4   'to be delivered'
delete from Products where Id=3
update Deliverers set DeliveryStatus='offline' where Id=1