from seleniumbase import BaseCase

class TestTodoClass(BaseCase):
    def test_view_todo(self):
        self.open("http://localhost:4200/#/summary")
        self.sleep(3)
        result = self.find_element("/html/body/app-root/app-summary/div[1]/div/table/tbody/tr[1]", by="css selector", timeout=None)
        self.assertIsNotNone(result)

        #view todo modal
        # result = self.find_element("/html/body/app-root/app-view-todo/div/div/div", by="css selector", timeout=None)
        # self.assertIsNone(result)
        self.click("/html/body/app-root/app-summary/div[1]/div/table/tbody/tr[1]/td[5]/button[1]")
        result = self.find_element("/html/body/app-root/app-view-todo/div/div/div", by="css selector", timeout=None)
        self.assertIsNotNone(result)
        
        #add tag modal
        # result = self.find_element("/html/body/app-root/app-add-tag/div/div/div", by="css selector", timeout=None)
        # self.assertIsNone(result)
        self.click("/html/body/app-root/app-view-todo/div/div/div/div[2]/div[2]/span/button")
        result = self.find_element("/html/body/app-root/app-add-tag/div/div/div", by="css selector", timeout=None)
        self.assertIsNotNone(result)
        
    def test_add_todo(self):
        self.open("http://localhost:4200/#/summary")
        self.sleep(3)
        result = self.find_element("/html/body/app-root/app-summary/div[1]/button", by="css selector", timeout=None)
        self.assertIsNotNone(result)

        #add todo modal
        # result = self.find_element("/html/body/app-root/app-add-todo/div/div/div", by="css selector", timeout=None)
        # self.assertIsNone(result)
        self.click("/html/body/app-root/app-summary/div[1]/button")
        result = self.find_element("/html/body/app-root/app-add-todo/div/div/div", by="css selector", timeout=None)
        self.assertIsNotNone(result)
        
        #add tag modal
        # result = self.find_element("/html/body/app-root/app-add-tag/div/div/div", by="css selector", timeout=None)
        # self.assertIsNone(result)
        self.click("/html/body/app-root/app-add-todo/div/div/div/form/div[1]/div[2]/span/button")
        result = self.find_element("/html/body/app-root/app-add-tag/div/div/div", by="css selector", timeout=None)
        self.assertIsNotNone(result)

    def test_delete_todo(self):
        self.open("http://localhost:4200/#/summary")
        self.sleep(3)
        result = self.find_element("/html/body/app-root/app-summary/div[1]/div/table/tbody/tr[1]", by="css selector", timeout=None)
        self.assertIsNotNone(result)

        #view todo modal
        # result = self.find_element("/html/body/app-root/app-summary/div[2]/div/div", by="css selector", timeout=None)
        # self.assertIsNone(result)
        self.click("/html/body/app-root/app-summary/div[1]/div/table/tbody/tr[1]/td[5]/button[2]")
        result = self.find_element("/html/body/app-root/app-summary/div[2]/div/div", by="css selector", timeout=None)
        self.assertIsNotNone(result)
        
        #delete button
        result = self.find_element("/html/body/app-root/app-summary/div[2]/div/div/div[3]/button[1]", by="css selector", timeout=None)
        self.assertIsNotNone(result)