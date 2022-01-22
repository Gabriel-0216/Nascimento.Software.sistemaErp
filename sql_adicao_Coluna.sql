use Erp_V3

-- Add a new column '[NewColumnName]' to table '[TableName]' in schema '[dbo]'
ALTER TABLE [dbo].[Order_Product]
    ADD [ProductQuantity]  int  NOT NULL /*new_column_nullability*/
GO