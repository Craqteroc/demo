   M S B A M L   `   `   `  ����B  >WpfApp3, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp3WpfApp3  5   	       WpfApp3.MainDtgWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WpfApp3     5   	     � Title$  MainDtgWindow��5   	   $	��450��6   $	��800��6,    � WindowStartupLocation$ CenterScreen=�68   .��5	      � .+�5
   
   ��  �� Orientation$ 
Horizontal=�6   $��Right=�6.   $	��Top=�6J   $��0,0,60,0q�6c   .+�5      �� $!��Введите модель��6   5      �� 5   
   .+�5      �� $ 
Horizontal=�6   $��Right=�62   $��Bottom=�6N   $��0,0,20,0q�6i   .+�5      �� -   5         ��Name$ 
AddRequest��5      $	��200��6F   $��25��6R   $��20q�6^   $#��Добавить заявку��6j   �� 5      -   5        $ EditRequest��5      $	��200��6H   $��25��6T   $��20q�6`   $-��'Редактировать заявку��6l   �� 5      -   5         ��Name$ SerchBox��5      $��25��6,   $	��200��68   6D   5      �� 5      $ 
Horizontal=�6   $��Right=�62   $��Bottom=�6N   .+�5      �� -   5        $ 
SaveReport��5      $	��200��6F   $#��Сохранить отчет��6R   $��25��6l   $��	0,0,20,20q�6x   �� 5      -   5        $ SaveqQR��5      $	��200��6@   $��Сохранить Qr��6L   $��25��6c   $��	0,0,20,20q�6o   5      &   System.Windows.Controls.DataGrid  5      -   5          Name$ mainDtg��5   3      AutoGenerateColumns .     
IsReadOnly .    Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn	   Header$	 	requestID��6G   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 6)   �� *	requestID+6G     5      $	 Дата начала��6G   
 6)   �� *	startDate+6G     5      $	 Тип��6E   
 6)   �� *	carType+6E     5      $	 Модель��6F   
 6)   �� *
carModel+6F     5      $	 Проблемы��6P   
 6)   �� *problemDescryption+6P     5       $!	 Статус ремонта��6K   
 6)   �� *requestStatus+6K     5!      $#	 Дата завершения��6L   
 6)   �� *completionDate+6L     5"      $	 Этап ремонта��6I   
 6)   �� *repairParts+6I     5#      $	 Мастер��6D   
 6)   �� *userID+6D     5$      $	 Заказчик��6E   
 6)   �� *	userID2+6E   5&      5'      5)      5*      