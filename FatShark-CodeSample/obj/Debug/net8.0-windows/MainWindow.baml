   M S B A M L   `   `   `  ����N  JFatShark-CodeSample, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:FatShark_CodeSampleFatShark_CodeSample  5   	   $    FatShark_CodeSample.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:FatShark_CodeSample     5   	   -   5          � Title$  
MainWindow��5   	   ��6   �� *WindowHeight+��6<   �� *WindowWidth+.��5	      � 
���   �.+�5
   
   ]� $��Stretch=�6   $��Stretch=�62   $
��auto��6N   $
��auto��6[   .+�5      �� $��Stretch=�6   $
��auto��67   .+�5      � -   5         �Name$ ToolBarGrid��5      $��Stretch=�6,   $	��Top=�6J   $��50��6b   
���   �.+�5      ��  �� Orientation$ 
Horizontal=�6!   $
��Left=�6:   $
��auto��6U   .+�5      �� -   5         ��Name$ SelectFilePathButton��5   !   .��5      �� -   5        $��DataImportFilePathTb��5   (   
���   � �� TextAlignment$ Center=�6{   $
��Left=�6�   $��5, 5,5,5q�6�   ��6D   �� *DataImportFilePath+6�   5      �� 5      -   5        $ ImportButton��5   !   .��5      �� $��Import Data��6(   
���   �$ Center=�6N   $��Right=�6e   $��5, 5,5,5q�6�   6�   5      �� 5      -   5        $ ProcessPostCodesButton��5   !   .��5      �� $��Determine Master Cluster��6(   
���   �$ Center=�6[   $��Right=�6r   $��5, 5,5,5q�6�   6�   5      5      5      � 5      -   5        $ Titles��5      $��Stretch=�6'   $	��Top=�6E   $
��auto��6]   $
��auto��6j   
���   �.+�5      �� -   5        $��MasterClusterTb��5      
���   �
�������$��30(�5      $
 Left=�5      $
��Left=�5       $��Center=�5!      ��5"      ��  �� Path$ MasterClusterName�5#      5$      5%      ]� 5&      $��	0,100,0,0q�6   .+�5'      � -	   5        $ ContentGrid��5'      $��Stretch=�6,   $��Center=�6J   $
��auto��6e   .+�5)      � $��Stretch=�6   $
��auto��69   $
��auto��6F   6V   &   System.Windows.Controls.DataGrid  5+      -
   5        $��PostCodeGridView��5+      $
��auto��67   $
��Left=�6D   $��Center=�6_   
���   �   	CellStyle 5,      �� 5-       �� 
TargetType*  $System.Windows.Controls.DataGridCell"  6$   .�5.   "   ��  �� Property � ��	 �� Value
	 �   �6O   �� 5/   "    � ��
	 �����6O   �� 50   "    � ��$	 0q�6D   6P   �� 51   "    � ��$	 13(�6=   6J   52      54        56      -   5        $��DataGridView��56      $
��auto��63   $��Right=�6@   $��Center=�6\   
���   � 57      �� 58      "  6$   .�59   "   ��  � ��
	 �   �6O   �� 5:   "    � ��
	 �����6O   �� 5;   "    � ��$	 0q�6D   6P   �� 5<   "    � ��$	 13(�6=   6J   5=      5?      5@      5A      5B      5E      5F      