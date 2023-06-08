

from seleniumbase import BaseCase

class TestSummaryClass(BaseCase):
    def test_table_row(self):
        self.open("http://localhost:4200/#/summary")
        self.sleep(3)
        result = self.find_element("/html/body/app-root/app-summary/div[1]/div/table/tbody/tr[1]", by="css selector", timeout=None)
        self.assertIsNotNone(result)

        result = self.find_element("/html/body/app-root/app-summary/div[1]/div/table/tbody/tr[1]/td[5]/button[1]", by="css selector", timeout=None)
        self.assertIsNotNone(result)

        result = self.find_element("/html/body/app-root/app-summary/div[1]/div/table/tbody/tr[1]/td[5]/button[2]", by="css selector", timeout=None)
        self.assertIsNotNone(result)
        
    def test_add_todo_button(self):
        self.open("http://localhost:4200/#/summary")
        self.sleep(3)
        result = self.find_element("/html/body/app-root/app-summary/div[1]/button", by="css selector", timeout=None)
        self.assertIsNotNone(result)